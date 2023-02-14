using Enterprise.Solution.API.Helpers;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

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
        /// List all/filter/search/page Authors
        /// </summary>
        /// <param name="filter">Nullable filter for name</param>
        /// <param name="search">Nullable search</param>
        /// <param name="pageNumber">Non-null pageNumber</param>
        /// <param name="pageSize">Non-null pageSize</param>
        /// <param name="includeBooks">Nullable includeBooks</param>
        /// <returns code="200">Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> ListAllAsync(
            [FromQuery] string? filter,
            string? search,
            int pageNumber = 1,
            int pageSize = 10,
            bool includeBooks = false
        )
        {
            var response = await AuthorService.ListAllAsync(filter, search, pageNumber, pageSize, includeBooks);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.PaginationMetadata));

            return Ok(Mapper.Map<IReadOnlyList<AuthorDTO>>(response.Entities));
        }

        /// <summary>
        /// Get Author by Id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="includeBooks">Non-null includeBooks</param>
        /// <returns code="200">Author</returns>
        [HttpGet("{id}", Name = "GetAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id, bool includeBooks = true)
        {
            var include = new List<string>();
            if (includeBooks.Equals(true))
            {
                include.Add("Books");
            }
            var author = await AuthorService.GetByIdAsync(id, includeBooks);
            if (author == null)
            {
                Logger.LogInformation($"Author with id {id} not found.");
                return NotFound();
            }

            return Ok(Mapper.Map<AuthorDTO>(author));
        }

        /// <summary>
        /// Create Author
        /// </summary>
        /// <param name="authorDTO">Non-null authorDTO</param>
        /// <returns code="201">Created Author</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] AuthorDTO authorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author author = Mapper.Map<Author>(authorDTO);

            var createdAuthor = await AuthorService.AddAsync(author);

            var createdAuthorDTO = Mapper.Map<AuthorDTO>(createdAuthor);

            return CreatedAtRoute("GetAuthor",
                new
                {
                    id = createdAuthorDTO.Id,
                }, createdAuthorDTO);
        }

        /// <summary>
        /// Update Author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="authorDTO">Non-null authorDTO</param>
        /// <returns code="204">No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(
            int id,
            [FromBody] AuthorDTO authorDTO
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorExists = await AuthorService.ExistsAsync(id);
            if (!authorExists)
            {
                Logger.LogInformation($"Author with id {id} not found.");
                return NotFound();
            }
            if (!id.Equals(authorDTO.Id))
            {
                return BadRequest($"Incorrect id for author with id {id}.");
            }

            var author = await AuthorService.GetByIdAsync(id);
            if (author != null)
            {
                Mapper.Map(authorDTO, author);
                await AuthorService.UpdateAsync(author);
                return Accepted();
            }

            return BadRequest($"An error occured while trying to update author with id {id}.");
        }

        /// <summary>
        /// Patch Author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">jsonPatchDocument</param>
        /// <returns code="204">No content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAsync(
            int id,
            [FromBody] JsonPatchDocument<AuthorDTO> jsonPatchDocument
            )
        {
            var authorExists = await AuthorService.ExistsAsync(id);
            if (!authorExists)
            {
                Logger.LogInformation($"Author with id {id} not found.");
                return NotFound();
            }

            var author = await AuthorService.GetByIdAsync(id);
            if (author == null)
            {
                Logger.LogInformation($"Author with id {id} was not found.");
                return NotFound();
            }

            var patchedAuthor = Mapper.Map<AuthorDTO>(author);

            jsonPatchDocument.ApplyTo(patchedAuthor, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(patchedAuthor))
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(patchedAuthor, author);

            await AuthorService.UpdateAsync(author);

            return NoContent();
        }

        /// <summary>
        /// Delete Author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="204">No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAuthorAsync(int id)
        {
            var authorExists = await AuthorService.ExistsAsync(id);
            if (!authorExists)
            {
                Logger.LogInformation($"Author with id {id} was not found.");
                return NotFound();
            }

            await AuthorService.DeleteAsync(id);

            return NoContent();
        }
    }
}
