using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.API.Controllers.Common;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Service.QueryParams;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;
using System.Net.Http.Headers;
using System.Text;

namespace Enterprise.Solution.API.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController : BaseController<UserController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public UserController(IOptions<SolutionSettings> solutionSettings, IMediator mediator) : base(solutionSettings, mediator) { }

        /// <summary>
        /// List All Users
        /// </summary>
        /// <returns code="200">List of Users</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserDTO_Response>>> ListAllAsync([FromQuery] UserPagedQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListAllUsersQuery(queryParams), cancellationToken);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response!.PaginationMetadata));
                return Ok(Mapper.Map<IReadOnlyList<UserDTO_Response>>(response.Entities));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get an User by Id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="200">Found User</returns>
        [HttpGet("{id}", Name = $"Get{nameof(User)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var dto = await base._mediator!.Send(new GetUserByIdQuery(id));
                return Ok(dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Create an User
        /// </summary>
        /// <param name="dto">Non-null userDTO</param>
        /// <returns code="201">Created User</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] UserDTO_Request dto)
        {
            try
            {
                var response = await _mediator.Send(new AddUserCommand(ModelState, dto));
                return CreatedAtRoute($"Get{nameof(User)}", new { id = response.Id }, response);
            }
            catch (NotAddedException ex)
            {
                return NotFound(ex);
            }
            catch (InvalidModelException ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update an User
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="dto">Non-null dto</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserDTO_Request dto)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var bearer);
                string token = bearer.ToString().Replace("Bearer ", "");

                var user = await GetKeycloakUser(token, dto.KeycloakUniqueIdentifier.GetValueOrDefault());

                if (user != null)
                {
                    if (await UpdateKeycloakUser(token, user, dto))
                    {
                        await _mediator.Send(new UpdateUserCommand(id, ModelState, dto));
                        return NoContent();
                    }

                    return BadRequest();
                }

                return BadRequest();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (NotUpdatedException ex)
            {
                return BadRequest(ex);
            }
            catch (InvalidModelException ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Patch an User
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">Non-null jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<UserDTO_Request> jsonPatchDocument)
        {
            try
            {
                await _mediator.Send(new PatchUserCommand(id, ModelState,
                    new Func<object, bool>((object patchResult) => TryValidateModel(patchResult)),
                    jsonPatchDocument));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (NotPatchedException ex)
            {
                return BadRequest(ex);
            }
            catch (InvalidModelException ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete an User
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="204">No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(id));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (NotDeletedException ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task<KeycloakUser?> GetKeycloakUser(string token, Guid KeycloakUniqueIdentifier)
        {
            string endPoint = base._solutionSettings.Authentication.Schemes.Keycloak.Users;
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var content = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<KeycloakUser>>(response.Content.ReadAsStringAsync().Result);

                return content!.FirstOrDefault(x => x.id.Equals(KeycloakUniqueIdentifier));
            }
            return null;
        }

        private async Task<bool> UpdateKeycloakUser(string token, KeycloakUser user, UserDTO_Request dto)
        {
            string endPoint = base._solutionSettings.Authentication.Schemes.Keycloak.Users + "/" + user.id;
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var data = new[]
            {
                new KeyValuePair<string, string>("id", user.id.ToString()),
                //new KeyValuePair<string, string>("username", user.username!),
                //new KeyValuePair<string, string>("enabled", user.enabled.ToString()),
                //new KeyValuePair<string, string>("totp", user.totp.ToString()),
                //new KeyValuePair<string, string>("emailVerified", user.emailVerified.ToString()),
                new KeyValuePair<string, string>("firstName", dto.FirstName ?? user.firstName!),
                new KeyValuePair<string, string>("lastName", dto.LastName ?? user.lastName!),
                new KeyValuePair<string, string>("email", dto.EmailAddress ?? user.email!),
                //new KeyValuePair<string, string>("disableableCredentialTypes", user.disableableCredentialTypes.ToString()!),
                //new KeyValuePair<string, string>("requiredActions", user.requiredActions.ToString()!),
                //new KeyValuePair<string, string>("notBefore", user.notBefore.ToString()),
                //new KeyValuePair<string, string>("access", user.access!.ToString()!),
            };

            //var response = await client.PutAsync(endPoint, new FormUrlEncodedContent(data));

            user.firstName = dto.FirstName;
            user.lastName = dto.LastName;
            user.email = dto.EmailAddress;

            var objAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(endPoint, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        private class KeycloakUserAccess
        {
            public bool manageGroupMembership { get; set; }
            public bool view { get; set; }
            public bool mapRoles { get; set; }
            public bool impersonate { get; set; }
            public bool manage { get; set; }
        }
        private class KeycloakUser
        {
            public Guid id { get; set; }
            //public long createdTimestamp { get; set; }
            public string? username { get; set; }
            public bool enabled { get; set; }
            public bool totp { get; set; }
            public bool emailVerified { get; set; }
            public string? firstName { get; set; }
            public string? lastName { get; set; }
            public string? email { get; set; }
            public IEnumerable<object> disableableCredentialTypes { get; set; } = Enumerable.Empty<object>();
            public IEnumerable<object> requiredActions { get; set; } = Enumerable.Empty<object>();
            public int notBefore { get; set; }
            public KeycloakUserAccess? access { get; set; }
        }
    }
}