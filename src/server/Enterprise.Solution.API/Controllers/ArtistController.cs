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
    /// Artist Controller
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/artists")]
    public class ArtistController : BaseController<ArtistController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public ArtistController(IOptions<SolutionSettings> solutionSettings, IMediator mediator) : base(solutionSettings, mediator) { }

        /// <summary>
        /// List All Artists
        /// </summary>
        /// <returns code="200">List of Artists</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> ListAllAsync([FromQuery] ArtistPagedQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListAllArtistsQuery(queryParams), cancellationToken);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response!.PaginationMetadata));
                return Ok(Mapper.Map<IReadOnlyList<ArtistDTO>>(response.Entities));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get an Artist by Id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="200">Found Artist</returns>
        [HttpGet("{id}", Name = $"Get{nameof(Artist)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var dto = await base._mediator!.Send(new GetArtistByIdQuery(id));
                return Ok(dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Create an Artist
        /// </summary>
        /// <param name="dto">Non-null artistDTO</param>
        /// <returns code="201">Created Artist</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] ArtistDTO dto)
        {
            try
            {
                var response = await _mediator.Send(new AddArtistCommand(ModelState, dto));
                return CreatedAtRoute($"Get{nameof(Artist)}", new { id = response.Id }, response);
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
        /// Update an Artist
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="dto">Non-null dto</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ArtistDTO dto)
        {
            try
            {
                await _mediator.Send(new UpdateArtistCommand(id, ModelState, dto));
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
        /// Patch an Artist
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">Non-null jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ArtistDTO> jsonPatchDocument)
        {
            try
            {
                await _mediator.Send(new PatchArtistCommand(id, ModelState,
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
        /// Delete an Artist
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
                await _mediator.Send(new DeleteArtistCommand(id));
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
