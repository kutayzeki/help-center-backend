using FeedbackHub.Core.Services.CompanyService;
using FeedbackHub.Dtos.CompanyDto;
using FeedbackHub.Models.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FeedbackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;
        private readonly ICompanyService _companyService;

        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE_NUMBER = 1;

        public CompanyController(IStringLocalizer<LocalizerController> stringLocalizer, ICompanyService companyService)
        {
            _stringLocalizer = stringLocalizer;
            _companyService = companyService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _companyService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _companyService.GetById(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create model)
        {
            try
            {
                var response = await _companyService.Create(model.Name, model.Description, model.Email, model.PhoneNumber);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Update model)
        {


            try
            {
                var response = await _companyService.Update(model);
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
            await _companyService.Delete(id);
            return NoContent();
        }
    }

}
