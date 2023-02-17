using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Core.Services.MailService;
using HelpCenter.Models.Mail;
using Microsoft.AspNetCore.Mvc;

namespace HelpCenter.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mail;

        public MailController(IMailService mail)
        {
            _mail = mail;
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMailAsync(MailData mailData)
        {
            bool result = await _mail.SendAsync(mailData, new CancellationToken());

            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

         [HttpPost("sendwelcomemail")]
        public async Task<IActionResult> SendWelcomeMailAsync(WelcomeMail welcomeMail)
        {

            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { welcomeMail.Email }, 
                "Welcome to our platform", 
                _mail.GetEmailTemplate("welcome", welcomeMail));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                var model = new ApiResponseViewModel
                {
                    Id = welcomeMail.Email,
                    IsSuccess = true,
                    Message = "Mail has successfully been sent using template."
                };
                return Ok(model);
            }
            else
            {
                var model = new ApiResponseViewModel
                {
                    Id = welcomeMail.Email,
                    IsSuccess = false,
                    Message = "An error occured. The Mail could not be sent."
                };
                return Ok(model);
            }
        }
    }
}
