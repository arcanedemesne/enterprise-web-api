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
    /// Cover Controller
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/covers")]
    public class CoverController : BaseController<CoverController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public CoverController(IOptions<SolutionSettings> solutionSettings, IMediator mediator) : base(solutionSettings, mediator) { }

        /// <summary>
        /// List All Covers
        /// </summary>
        /// <returns code="200">List of Covers</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CoverDTO_Response>>> ListAllAsync([FromQuery] CoverQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListAllCoversQuery(queryParams), cancellationToken);
                return Ok(Mapper.Map<IReadOnlyList<CoverDTO_Response>>(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// List Paged Covers
        /// </summary>
        /// <returns code="200">List of Covers</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CoverDTO_Response>>> ListPagedAsync([FromQuery] CoverPagedQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListPagedCoversQuery(queryParams), cancellationToken);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response!.PaginationMetadata));
                return Ok(Mapper.Map<IReadOnlyList<CoverDTO_Response>>(response.Entities));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get an Cover by Id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="queryParams">IncludeAuthor || IncludeCover || IncludeCoverAndArtists</param>
        /// <returns code="200">Found Cover</returns>
        [HttpGet("{id}", Name = $"Get{nameof(Cover)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id, [FromQuery] CoverQueryParams queryParams)
        {
            try
            {
                var dto = await base._mediator!.Send(new GetCoverByIdQuery(id, queryParams));
                return Ok(dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Create an Cover
        /// </summary>
        /// <param name="dto">Non-null coverDTO</param>
        /// <returns code="201">Created Cover</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] CoverDTO_Request dto)
        {
            try
            {
                var response = await _mediator.Send(new AddCoverCommand(ModelState, dto));
                return CreatedAtRoute($"Get{nameof(Cover)}", new { id = response.Id }, response);
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
        /// Update an Cover
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="dto">Non-null dto</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CoverDTO_Request dto)
        {
            try
            {
                await _mediator.Send(new UpdateCoverCommand(id, ModelState, dto));
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
        /// Patch an Cover
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">Non-null jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<CoverDTO_Request> jsonPatchDocument)
        {
            try
            {
                await _mediator.Send(new PatchCoverCommand(id, ModelState,
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
        /// Delete an Cover
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
                await _mediator.Send(new DeleteCoverCommand(id));
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
