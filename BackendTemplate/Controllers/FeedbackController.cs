using FeedbackHub.Core.Services.FeedbackService;
using FeedbackHub.Dtos.FeedbackDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackHub.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE_NUMBER = 1;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int pageNumber = DEFAULT_PAGE_NUMBER, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var result = await _feedbackService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _feedbackService.GetById(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackCreate model)
        {
            try
            {
                var response = await _feedbackService.Create(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FeedbackUpdate model)
        {
            try
            {
                var response = await _feedbackService.Update(model);
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
                var response = await _feedbackService.Delete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("upvote")]
        public async Task<IActionResult> CreateFeedbackUpvote([FromBody] CreateUpvote model)
        {
            try
            {
                var response = await _feedbackService.CreateFeedbackUpvote(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("removeupvote/{id}")]
        public async Task<IActionResult> DeleteFeedbackUpvote(Guid id)
        {
            try
            {
                var response = await _feedbackService.DeleteFeedbackUpvote(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
