﻿using Enterprise.Solution.API.Controllers.Common;
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
        /// List All Authors (SearchQuery for First/Last Name, Pagination, and collection expansion)
        /// </summary>
        /// <param name="queryParams">Not-null queryParams</param>
        /// <returns code="200">Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> ListAllAsync([FromQuery] AuthorPagedQueryParams queryParams)
        {
            var response = await AuthorService.ListAllAsync(
                SanitizePageNumber(queryParams.PageNumber),
                SanitizePageSize(queryParams.PageSize),
                queryParams.SearchQuery,
                queryParams.IncludeBooks ?? false,
                queryParams.IncludeBooksWithCover ?? false,
                queryParams.IncludeBooksWithCoverAndArtists ?? false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.PaginationMetadata));

            return Ok(Mapper.Map<IReadOnlyList<AuthorDTO>>(response.Entities));
        }

        /// <summary>
        /// Get Author by Id (collection expansion)
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="queryParams">Non-null queryParams</param>
        /// <returns code="200">Author</returns>
        [HttpGet("{id}", Name = "GetAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id, AuthorQueryParams queryParams)
        {
            var author = await AuthorService.GetByIdAsync(
                id,
                queryParams.IncludeBooks ?? false,
                queryParams.IncludeBooksWithCover ?? false,
                queryParams.IncludeBooksWithCoverAndArtists ?? false);

            if (author == null)
            {
                Logger.LogInformation($"Author with id {id} was not found.");
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
                Logger.LogInformation($"ModelState is invalid.");
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
        /// <returns code="204">No Content</returns>
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
                Logger.LogInformation($"ModelState is invalid.");
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
                var message = $"Incorrect id for author with id {id}.";
                Logger.LogInformation(message);
                return BadRequest(message);
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
        /// Patch Author
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
            [FromBody] JsonPatchDocument<AuthorDTO> jsonPatchDocument
            )
        {
            var authorExists = await AuthorService.ExistsAsync(id);
            if (!authorExists)
            {
                Logger.LogInformation($"Author with id {id} was not found.");
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
                Logger.LogInformation($"ModelState is invalid.");
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(patchedAuthor))
            {
                Logger.LogInformation($"ModelState Patch is invalid.");
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
