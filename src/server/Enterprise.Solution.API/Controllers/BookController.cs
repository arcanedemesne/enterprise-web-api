using Enterprise.Solution.API.Controllers.Common;
using Enterprise.Solution.API.Helpers.QueryParams;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

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
        /// List All Books (SearchQuery for Title, Pagination, and collection expansion)
        /// </summary>
        /// <param name="queryParams">Not-null queryParams</param>
        /// <returns code="200">Books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BookDTO>>> ListAllAsync([FromQuery] BookPagedQueryParams queryParams)
        {
            var response = await BookService.ListAllAsync(
                SanitizePageNumber(queryParams.PageNumber),
                SanitizePageSize(queryParams.PageSize),
                queryParams.SearchQuery,
                queryParams.IncludeAuthor ?? false,
                queryParams.IncludeCover ?? false,
                queryParams.IncludeCoverAndArtists ?? false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.PaginationMetadata));

            return Ok(Mapper.Map<IReadOnlyList<BookDTO>>(response.Entities));
        }

        /// <summary>
        /// Get Book by Id (collection expansion)
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="queryParams">Non-null queryParams</param>
        /// <returns code="200">Book</returns>
        [HttpGet("{id}", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id, BookQueryParams queryParams)
        {
            var book = await BookService.GetByIdAsync(
                id,
                queryParams.IncludeAuthor ?? false,
                queryParams.IncludeCover ?? false,
                queryParams.IncludeCoverAndArtists ?? false);

            if (book == null)
            {
                Logger.LogInformation($"Book with id {id} was not found.");
                return NotFound();
            }

            return Ok(Mapper.Map<BookDTO>(book));
        }

        /// <summary>
        /// Create Book
        /// </summary>
        /// <param name="bookDTO">Non-null bookDTO</param>
        /// <returns code="201">Created Book</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] BookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"ModelState is invalid.");
                return BadRequest(ModelState);
            }

            Book book = Mapper.Map<Book>(bookDTO);

            var createdBook = await BookService.AddAsync(book);

            var createdBookDTO = Mapper.Map<BookDTO>(createdBook);

            return CreatedAtRoute("GetBook",
                new
                {
                    id = createdBookDTO.Id,
                }, createdBookDTO);
        }

        /// <summary>
        /// Update Book
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="bookDTO">Non-null bookDTO</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(
            int id,
            [FromBody] BookDTO bookDTO
        )
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"ModelState is invalid.");
                return BadRequest(ModelState);
            }

            var bookExists = await BookService.ExistsAsync(id);
            if (!bookExists)
            {
                Logger.LogInformation($"Book with id {id} not found.");
                return NotFound();
            }
            if (!id.Equals(bookDTO.Id))
            {
                var message = $"Incorrect id for book with id {id}.";
                Logger.LogInformation(message);
                return BadRequest(message);
            }

            var book = await BookService.GetByIdAsync(id);
            if (book != null)
            {
                Mapper.Map(bookDTO, book);
                await BookService.UpdateAsync(book);
                return NoContent();
            }

            return BadRequest($"An error occured while trying to update book with id {id}.");
        }

        /// <summary>
        /// Patch Book
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">Non-null jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(
            int id,
            [FromBody] JsonPatchDocument<BookDTO> jsonPatchDocument
            )
        {
            var bookExists = await BookService.ExistsAsync(id);
            if (!bookExists)
            {
                Logger.LogInformation($"Book with id {id} was not found.");
                return NotFound();
            }

            var book = await BookService.GetByIdAsync(id);
            if (book == null)
            {
                Logger.LogInformation($"Book with id {id} was not found.");
                return NotFound();
            }

            var patchedBook = Mapper.Map<BookDTO>(book);

            jsonPatchDocument.ApplyTo(patchedBook, ModelState);

            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"ModelState is invalid.");
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(patchedBook))
            {
                Logger.LogInformation($"ModelState Patch is invalid.");
                return BadRequest(ModelState);
            }

            Mapper.Map(patchedBook, book);

            await BookService.UpdateAsync(book);

            return NoContent();
        }

        /// <summary>
        /// Delete Book
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="204">No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            var bookExists = await BookService.ExistsAsync(id);
            if (!bookExists)
            {
                Logger.LogInformation($"Book with id {id} was not found.");
                return NotFound();
            }

            await BookService.DeleteAsync(id);

            return NoContent();
        }
    }
}
