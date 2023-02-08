using AutoMapper;
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
    /// Item Controller
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/items")]

    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        /// <summary>
        /// ItemController Constructor
        /// </summary>
        /// <param name="itemService">Non-null IItemService</param>
        /// <param name="mapper">Non-null IMapper</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all/filter/search/page items
        /// </summary>
        /// <param name="name" type="string?">Nullable filter for name</param>
        /// <param name="searchQuery" type="string?">Nullable searchQuery</param>
        /// <param name="pageNumber" type="int">Non-null pageNumber</param>
        /// <param name="pageSize" type="int">Non-null pageSize</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItemsAsync(
        [FromQuery] string? name,
            string? searchQuery,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var (items, paginationMetadata) = await _itemService.GetItemsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<ItemResponseDto>>(items));
        }

        /// <summary>
        /// Get an item by id
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested item</response>
        [HttpGet("{id}", Name = "GetItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var item = await _itemService.GetItemAsync(id);
            if (item == null)
            {
                //_logger.LogInformation($"Item with id {itemId} not found.");
                return NotFound();
            }

            return Ok(_mapper.Map<ItemResponseDto>(item));
        }

        /// <summary>
        /// Create an item
        /// </summary>
        /// <param name="itemRequestDto">Non-null itemRequestDto</param>
        /// <returns code="201">Returns the created item</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemAsync(
            [FromBody] ItemRequestDto itemRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _mapper.Map<Item>(itemRequestDto);

            await _itemService.AddItemAsync(item);

            var createdItemDto = _mapper.Map<ItemResponseDto>(item);

            return CreatedAtRoute("GetItem",
                new
                {
                    id = createdItemDto.Id,
                }, createdItemDto);
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="itemResponseDto">Non-null itemRequestDto</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemAsync(
            int id,
            [FromBody] ItemResponseDto itemResponseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemExists = await _itemService.ItemExistsAsync(id);
            if (!itemExists)
            {
                //_logger.LogInformation($"Item with id {id} not found.");
                return NotFound();
            }
            if (!id.Equals(itemResponseDto.Id))
            {
                return BadRequest($"Incorrect id for item with id {id}.");
            }

            var item = await _itemService.GetItemAsync(id);
            if (item != null)
            {
                _mapper.Map(itemResponseDto, item);
                if (await _itemService.UpdateItemAsync(item))
                {
                    return NoContent();
                };
            }

            return BadRequest($"An error occured while trying to update item with id {id}.");
        }

        /// <summary>
        /// Patch an item
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <param name="jsonPatchDocument">jsonPatchDocument</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchItemAsync(
            int id,
            [FromBody] JsonPatchDocument<ItemResponseDto> jsonPatchDocument)
        {
            var itemExists = await _itemService.ItemExistsAsync(id);
            if (!itemExists)
            {
                //_logger.LogInformation($"Item with id {id} not found.");
                return NotFound();
            }

            var item = await _itemService.GetItemAsync(id);
            if (item == null)
            {
                //_logger.LogInformation($"Item with id {id} was not found.");
                return NotFound();
            }

            var patchedItem = _mapper.Map<ItemResponseDto>(item);

            jsonPatchDocument.ApplyTo(patchedItem, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(patchedItem))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(patchedItem, item);

            await _itemService.UpdateItemAsync(item);

            return NoContent();
        }

        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="id">Non-null id</param>
        /// <returns code="204">IActionResult</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var item = await _itemService.GetItemAsync(id);
            if (item == null)
            {
                //_logger.LogInformation($"Item with id {id} was not found.");
                return NotFound();
            }

            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
