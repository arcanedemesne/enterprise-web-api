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
    /// Book Controller
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/books")]
    public class BookController : BaseController<BookController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public BookController(IOptions<SolutionSettings> solutionSettings, IMediator mediator) : base(solutionSettings, mediator) { }

        /// <summary>
        /// List All Books
        /// </summary>
        /// <returns code="200">List of Books</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BookDTO_Response>>> ListAllAsync([FromQuery] BookQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListAllBooksQuery(queryParams), cancellationToken);
                return Ok(Mapper.Map<IReadOnlyList<BookDTO_Response>>(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// List Paged Books
        /// </summary>
        /// <returns code="200">List of Books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BookDTO_Response>>> ListPagedAsync([FromQuery] BookPagedQueryParams queryParams, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base._mediator!.Send(new ListPagedBooksQuery(queryParams), cancellationToken);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response!.PaginationMetadata));
                return Ok(Mapper.Map<IReadOnlyList<BookDTO_Response>>(response.Entities));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get an Book by Id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="queryParams">IncludeAuthor || IncludeCover || IncludeCoverAndArtists</param>
        /// <returns code="200">Found Book</returns>
        [HttpGet("{id}", Name = $"Get{nameof(Book)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id, [FromQuery] BookQueryParams queryParams)
        {
            try
            {
                var dto = await base._mediator!.Send(new GetBookByIdQuery(id, queryParams));
                return Ok(dto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Create an Book
        /// </summary>
        /// <param name="dto">Non-null bookDTO</param>
        /// <returns code="201">Created Book</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] BookDTO_Request dto)
        {
            try
            {
                var response = await _mediator.Send(new AddBookCommand(ModelState, dto));
                return CreatedAtRoute($"Get{nameof(Book)}", new { id = response.Id }, response);
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
        /// Update an Book
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="dto">Non-null dto</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] BookDTO_Request dto)
        {
            try
            {
                await _mediator.Send(new UpdateBookCommand(id, ModelState, dto));
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
        /// Patch an Book
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">Non-null jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<BookDTO_Request> jsonPatchDocument)
        {
            try
            {
                await _mediator.Send(new PatchBookCommand(id, ModelState,
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
        /// Delete an Book
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
                await _mediator.Send(new DeleteBookCommand(id));
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
