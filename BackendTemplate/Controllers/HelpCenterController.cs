using HelpCenter.Core.Services.HelpCenterService;
using HelpCenter.Dtos.HelpCenterDto;
using HelpCenter.Models.HelpCenter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpCenter.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class HelpCenterController : ControllerBase
    {
        private readonly IHelpCenterService _helpCenterService;

        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE_NUMBER = 1;

        public HelpCenterController(IHelpCenterService helpCenterService)
        {
            _helpCenterService = helpCenterService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _helpCenterService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _helpCenterService.GetById(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }
        [HttpGet("GetHelpCentersByProduct")]
        public async Task<IActionResult> GetHelpCentersByProduct(Guid productId, int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _helpCenterService.GetHelpCentersByProductId(productId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HelpCenterCreate model)
        {
            try
            {
                var response = await _helpCenterService.Create(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] HelpCenterUpdate model)
        {
            try
            {
                var response = await _helpCenterService.Update(model);
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
                var response = await _helpCenterService.Delete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
}
