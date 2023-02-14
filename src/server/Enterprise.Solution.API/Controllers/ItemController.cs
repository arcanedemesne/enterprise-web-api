using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Entities;
using Enterprise.Solution.Data.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Enterprise.Solution.API.Controllers
{
    /// <summary>
    /// Item Controller
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/items")]

    public class ItemController : BaseController<ItemController>
    {
        /// <summary>
        /// Get all paged items
        /// </summary>
        /// <param name="pageNumber">Non-null pageNumber</param>
        /// <param name="pageSize">Non-null pageSize</param>
        /// <returns code="200">Requested Items</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> ListAllAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var response = await ItemService.ListAllAsync(pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.PaginationMetadata));

            return Ok(Mapper.Map<IReadOnlyList<ItemDTO>>(response.Entities));
        }

        /// <summary>
        /// Get an item by id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Requested Item</response>
        [HttpGet("{id}", Name = "GetItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var item = await ItemService.GetByIdAsync(id);
            if (item == null)
            {
                Logger.LogInformation($"Item with id {id} not found.");
                return NotFound();
            }

            return Ok(Mapper.Map<ItemDTO>(item));
        }

        /// <summary>
        /// Create an item
        /// </summary>
        /// <param name="itemDTO">Non-null itemDTO</param>
        /// <returns code="201">Created Item</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync(
            [FromBody] ItemDTO itemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = Mapper.Map<Item>(itemDTO);

            await ItemService.AddAsync(item);

            var createdItemDTO = Mapper.Map<ItemDTO>(item);

            return CreatedAtRoute("GetItem",
                new
                {
                    id = createdItemDTO.Id,
                }, createdItemDTO);
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="itemDTO">Non-null itemRequestDto</param>
        /// <returns code="204">No Content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(
            int id,
            [FromBody] ItemDTO itemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemExists = await ItemService.ExistsAsync(id);
            if (!itemExists)
            {
                //_logger.LogInformation($"Item with id {id} not found.");
                return NotFound();
            }
            if (!id.Equals(itemDTO.Id))
            {
                return BadRequest($"Incorrect id for item with id {id}.");
            }

            var item = await ItemService.GetByIdAsync(id);
            if (item != null)
            {
                Mapper.Map(itemDTO, item);
                await ItemService.UpdateAsync(item);

                return NoContent();
            }

            return BadRequest($"An error occured while trying to update item with id {id}.");
        }

        /// <summary>
        /// Patch an item
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">jsonPatchDocument</param>
        /// <returns code="204">No Content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchItemAsync(
            int id,
            [FromBody] JsonPatchDocument<ItemDTO> jsonPatchDocument)
        {
            var itemExists = await ItemService.ExistsAsync(id);
            if (!itemExists)
            {
                //_logger.LogInformation($"Item with id {id} not found.");
                return NotFound();
            }

            var item = await ItemService.GetByIdAsync(id);
            if (item == null)
            {
                //_logger.LogInformation($"Item with id {id} was not found.");
                return NotFound();
            }

            var patchedItem = Mapper.Map<ItemDTO>(item);

            jsonPatchDocument.ApplyTo(patchedItem, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(patchedItem))
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(patchedItem, item);

            await ItemService.UpdateAsync(item);

            return NoContent();
        }

        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="204">No Content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var item = await ItemService.GetByIdAsync(id);
            if (item == null)
            {
                //_logger.LogInformation($"Item with id {id} was not found.");
                return NotFound();
            }

            await ItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
