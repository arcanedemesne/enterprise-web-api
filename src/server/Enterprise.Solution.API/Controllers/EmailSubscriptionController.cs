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
using Enterprise.Solution.Data.Models;

namespace Enterprise.Solution.API.Controllers
{
    /// <summary>
    /// Email Subscription Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/email-subscriptions")]
    public class EmailSubscriptionController : BaseController<EmailSubscriptionController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public EmailSubscriptionController(IOptions<SolutionSettings> solutionSettings, IMediator mediator) : base(solutionSettings, mediator) { }

        /// <summary>
        /// List All Email Subscriptions
        /// </summary>
        /// <returns code="200">List of Email Subscriptions</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmailSubscriptionDTO>>> ListAllAsync([FromQuery] EmailSubscriptionPagedQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListAllEmailSubscriptionsQuery(queryParams), cancellationToken);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response!.PaginationMetadata));
                return Ok(Mapper.Map<IReadOnlyList<EmailSubscriptionDTO>>(response.Entities));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get an Email Subscription by Id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="200">Found Email Subscription</returns>
        [HttpGet("{id}", Name = $"Get{nameof(EmailSubscription)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var dto = await base._mediator!.Send(new GetEmailSubscriptionByIdQuery(id));
                return Ok(dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Create an Email Subscription
        /// </summary>
        /// <param name="dto">Non-null emailSubscriptionDTO</param>
        /// <returns code="201">Created Email Subscription</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] EmailSubscriptionDTO dto)
        {
            try
            {
                var response = await _mediator.Send(new AddEmailSubscriptionCommand(ModelState, dto));
                return CreatedAtRoute($"Get{nameof(EmailSubscription)}", new { id = response.Id }, response);
            }
            catch (NotCreatedException ex)
            {
                return NotFound(ex);
            }
            catch (InvalidModelException ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update an Email Subscription
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="dto">Non-null dto</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] EmailSubscriptionDTO dto)
        {
            try
            {
                await _mediator.Send(new UpdateEmailSubscriptionCommand(id, ModelState, dto));
                return NoContent();
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
        /// Patch an Email Subscription
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">Non-null jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<EmailSubscriptionDTO> jsonPatchDocument)
        {
            try
            {
                await _mediator.Send(new PatchEmailSubscriptionCommand(id, ModelState,
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
        /// Delete an Email Subscription
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
                await _mediator.Send(new DeleteEmailSubscriptionCommand(id));
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
    }
}