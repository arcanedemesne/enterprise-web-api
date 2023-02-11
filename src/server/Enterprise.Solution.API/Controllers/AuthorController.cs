using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Entities;
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
        /// Get all/filter/search/page authors
        /// </summary>
        /// <param name="filter" type="string?">Nullable filter for name</param>
        /// <param name="searchQuery" type="string?">Nullable searchQuery</param>
        /// <param name="pageNumber" type="int">Non-null pageNumber</param>
        /// <param name="pageSize" type="int">Non-null pageSize</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> ListAllAsync(
            [FromQuery] string? filter,
            string? searchQuery,
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            var (authors, paginationMetadata) = await AuthorService.ListAllAsync(filter, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(Mapper.Map<IEnumerable<AuthorDTO>>(authors));
        }

        /// <summary>
        /// Get an author by id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="includeBooks">Nullable includeBooks</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested author</response>
        [HttpGet("{id}", Name = "GetAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id, bool? includeBooks = true)
        {
            var include = new List<string>();
            if (includeBooks.Equals(true))
            {
                include.Add("Books");
            }
            var author = await AuthorService.GetByIdAsync(id);
            if (author == null)
            {
                Logger.LogInformation($"Author with id {id} not found.");
                return NotFound();
            }

            return Ok(Mapper.Map<AuthorDTO>(author));
        }

        /// <summary>
        /// Create an author
        /// </summary>
        /// <param name="authorDTO">Non-null authorDTO</param>
        /// <returns code="201">Returns the created author</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAuthorAsync([FromBody] AuthorDTO authorDTO)
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
        /// Update an author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="authorDTO">Non-null authorDTO</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAuthorAsync(
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
                //_logger.LogInformation($"Author with id {id} not found.");
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
                return NoContent();
            }

            return BadRequest($"An error occured while trying to update author with id {id}.");
        }

        /// <summary>
        /// Patch an author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">jsonPatchDocument</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchAuthorAsync(
            int id,
            [FromBody] JsonPatchDocument<AuthorDTO> jsonPatchDocument
            )
        {
            var authorExists = await AuthorService.ExistsAsync(id);
            if (!authorExists)
            {
                //_logger.LogInformation($"Author with id {id} not found.");
                return NotFound();
            }

            var author = await AuthorService.GetByIdAsync(id);
            if (author == null)
            {
                //_logger.LogInformation($"Author with id {id} was not found.");
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
        /// Delete an author
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="204">IActionResult</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAuthorAsync(int id)
        {
            var authorExists = await AuthorService.ExistsAsync(id);
            if (authorExists == null)
            {
                //_logger.LogInformation($"Author with id {id} was not found.");
                return NotFound();
            }

            await AuthorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
