using HelpCenter.Core.Services.ItemService;
using HelpCenter.Dtos.ItemDto;
using HelpCenter.Models.Item;
using Item.Dtos.ItemDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpCenter.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _ItemService;

        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE_NUMBER = 1;

        public ItemController(IItemService ItemService)
        {
            _ItemService = ItemService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _ItemService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _ItemService.GetById(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }
        [HttpGet("GetItemsBySection")]
        public async Task<IActionResult> GetItemsByProduct(Guid helpCenterId, int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _ItemService.GetItemsBySectionId(helpCenterId,pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemCreate model)
        {
            try
            {
                var response = await _ItemService.Create(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ItemUpdate model)
        {
            try
            {
                var response = await _ItemService.Update(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _ItemService.Delete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
}
