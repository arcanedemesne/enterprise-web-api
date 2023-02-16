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
        /// List All Artists (SearchQuery for First/Last Name, Pagination, and collection expansion)
        /// </summary>
        /// <param name="queryParams">Not-null queryParams</param>
        /// <returns code="200">Artists</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> ListAllAsync([FromQuery] ArtistPagedQueryParams queryParams)
        {
            var response = await ArtistService.ListAllAsync(
                SanitizePageNumber(queryParams.PageNumber),
                SanitizePageSize(queryParams.PageSize),
                queryParams.SearchQuery,
                queryParams.IncludeCovers ?? false,
                queryParams.IncludeCoversWithBook ?? false,
                queryParams.IncludeCoversWithBookAndAuthor ?? false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.PaginationMetadata));

            return Ok(Mapper.Map<IReadOnlyList<ArtistDTO>>(response.Entities));
        }

        /// <summary>
        /// Get Artist by Id (collection expansion)
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="queryParams">Non-null queryParams</param>
        /// <returns code="200">Artist</returns>
        [HttpGet("{id}", Name = "GetArtist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id, ArtistQueryParams queryParams)
        {
            var artist = await ArtistService.GetByIdAsync(
                id,
                queryParams.IncludeCovers ?? false,
                queryParams.IncludeCoversWithBook ?? false,
                queryParams.IncludeCoversWithBookAndAuthor ?? false);

            if (artist == null)
            {
                Logger.LogInformation($"Artist with id {id} was not found.");
                return NotFound();
            }

            return Ok(Mapper.Map<ArtistDTO>(artist));
        }

        /// <summary>
        /// Create Artist
        /// </summary>
        /// <param name="artistDTO">Non-null artistDTO</param>
        /// <returns code="201">Created Artist</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] ArtistDTO artistDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"ModelState is invalid.");
                return BadRequest(ModelState);
            }

            Artist artist = Mapper.Map<Artist>(artistDTO);

            var createdArtist = await ArtistService.AddAsync(artist);

            var createdArtistDTO = Mapper.Map<ArtistDTO>(createdArtist);

            return CreatedAtRoute("GetArtist",
                new
                {
                    id = createdArtistDTO.Id,
                }, createdArtistDTO);
        }

        /// <summary>
        /// Update Artist
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="artistDTO">Non-null artistDTO</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(
            int id,
            [FromBody] ArtistDTO artistDTO
        )
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"ModelState is invalid.");
                return BadRequest(ModelState);
            }

            var artistExists = await ArtistService.ExistsAsync(id);
            if (!artistExists)
            {
                Logger.LogInformation($"Artist with id {id} not found.");
                return NotFound();
            }
            if (!id.Equals(artistDTO.Id))
            {
                var message = $"Incorrect id for artist with id {id}.";
                Logger.LogInformation(message);
                return BadRequest(message);
            }

            var artist = await ArtistService.GetByIdAsync(id);
            if (artist != null)
            {
                Mapper.Map(artistDTO, artist);
                await ArtistService.UpdateAsync(artist);
                return NoContent();
            }

            return BadRequest($"An error occured while trying to update artist with id {id}.");
        }

        /// <summary>
        /// Patch Artist
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
            [FromBody] JsonPatchDocument<ArtistDTO> jsonPatchDocument
            )
        {
            var artistExists = await ArtistService.ExistsAsync(id);
            if (!artistExists)
            {
                Logger.LogInformation($"Artist with id {id} was not found.");
                return NotFound();
            }

            var artist = await ArtistService.GetByIdAsync(id);
            if (artist == null)
            {
                Logger.LogInformation($"Artist with id {id} was not found.");
                return NotFound();
            }

            var patchedArtist = Mapper.Map<ArtistDTO>(artist);

            jsonPatchDocument.ApplyTo(patchedArtist, ModelState);

            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"ModelState is invalid.");
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(patchedArtist))
            {
                Logger.LogInformation($"ModelState Patch is invalid.");
                return BadRequest(ModelState);
            }

            Mapper.Map(patchedArtist, artist);

            await ArtistService.UpdateAsync(artist);

            return NoContent();
        }

        /// <summary>
        /// Delete Artist
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="204">No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteArtistAsync(int id)
        {
            var artistExists = await ArtistService.ExistsAsync(id);
            if (!artistExists)
            {
                Logger.LogInformation($"Artist with id {id} was not found.");
                return NotFound();
            }

            await ArtistService.DeleteAsync(id);

            return NoContent();
        }
    }
}
