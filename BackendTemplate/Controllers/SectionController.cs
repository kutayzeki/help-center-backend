using HelpCenter.Core.Services.SectionService;
using HelpCenter.Dtos.SectionDto;
using HelpCenter.Models.Section;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpCenter.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _SectionService;

        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE_NUMBER = 1;

        public SectionController(ISectionService SectionService)
        {
            _SectionService = SectionService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _SectionService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _SectionService.GetById(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }
        [HttpGet("GetSectionsByHelpCenter")]
        public async Task<IActionResult> GetSectionsByProduct(Guid helpCenterId, int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _SectionService.GetSectionsByHelpCenterId(helpCenterId,pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SectionCreate model)
        {
            try
            {
                var response = await _SectionService.Create(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SectionUpdate model)
        {
            try
            {
                var response = await _SectionService.Update(model);
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
                var response = await _SectionService.Delete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
}
