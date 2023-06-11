using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.API.Controllers.Common;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.QueryParams;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

namespace Enterprise.Solution.API.Controllers
{
    /// <summary>
    /// Author Controller
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/authors")]
    public class AuthorController : BaseController<AuthorController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public AuthorController(IOptions<SolutionSettings> solutionSettings, IMediator mediator) : base(solutionSettings, mediator) { }

        /// <summary>
        /// List All Authors
        /// </summary>
        /// <returns code="200">List of Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> ListAllAsync([FromQuery] AuthorPagedQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListAllAuthorsQuery(queryParams), cancellationToken);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response!.PaginationMetadata));
                return Ok(Mapper.Map<IReadOnlyList<AuthorDTO>>(response.Entities));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get an Author by Id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="200">Found Author</returns>
        [HttpGet("{id}", Name = $"Get{nameof(Author)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var dto = await base._mediator!.Send(new GetAuthorByIdQuery(id));
                return Ok(dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Create an Author
        /// </summary>
        /// <param name="dto">Non-null authorDTO</param>
        /// <returns code="201">Created Author</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] AuthorDTO dto)
        {
            try
            {
                var response = await _mediator.Send(new AddAuthorCommand(ModelState, dto));
                return CreatedAtRoute($"Get{nameof(Author)}", new { id = response.Id }, response);
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
        /// Update an Author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="dto">Non-null dto</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AuthorDTO dto)
        {
            try
            {
                await _mediator.Send(new UpdateAuthorCommand(id, ModelState, dto));
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
        /// Patch an Author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">Non-null jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<AuthorDTO> jsonPatchDocument)
        {
            try
            {
                await _mediator.Send(new PatchAuthorCommand(id, ModelState,
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
        /// Delete an Author
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
                await _mediator.Send(new DeleteAuthorCommand(id));
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
