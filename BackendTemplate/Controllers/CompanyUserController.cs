using FeedbackHub.Core.Services.CompanyUserService;
using FeedbackHub.Dtos.CompanyUserDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackHub.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CompanyUserController : ControllerBase
    {
        private readonly ICompanyUserService _productService;

        public CompanyUserController(ICompanyUserService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyUserCreate model)
        {
            try
            {
                var response = await _productService.Create(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid companyId, string userId)
        {
            try
            {
                var response = await _productService.Delete(companyId, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
