using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Core.Services.MailService;
using FeedbackHub.Core.Utilities;
using FeedbackHub.Models;
using FeedbackHub.Models.User;
using Ekip.Core.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackHub.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class IdentityController : ControllerBase
    {
        private readonly APIDbContext _context;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;

        public IdentityController(APIDbContext context,
                              IConfiguration configuration,
                              SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IMailService emailService)
        {
            _context = context;
            _config = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = emailService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            ApiResponseViewModel responseViewModel = new();

            try
            {
                #region Validate
                if (!ModelState.IsValid)
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = "Your information is incomplete, some fields were not submitted. Please fill in all fields.";

                    return BadRequest(responseViewModel);
                }

                ApplicationUser existsUser = await _userManager.FindByNameAsync(model.Email);

                if (existsUser != null)
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = "The user already exists.";

                    return BadRequest(responseViewModel);
                }
                #endregion

                // User information is set.
                ApplicationUser user = new()
                {
                    Email = model.Email.Trim(),
                    UserName = model.Email.Trim()
                };

                // The user is created.
                IdentityResult result = await _userManager.CreateAsync(user, model.Password.Trim());

                  
                if (result.Succeeded)
                {

                    bool roleExists = await _roleManager.RoleExistsAsync(_config[$"Roles:{model.Role}"]);
                    if (!roleExists)
                    {
                        IdentityRole role = new(_config[$"Roles:{model.Role}"])
                        {
                            NormalizedName = _config[$"Roles:{model.Role}"]
                        };
                        _roleManager.CreateAsync(role).Wait();
                    }


                    _userManager.AddToRoleAsync(user, _config["Roles:" + model.Role]).Wait();

                    //Send email
                    /*  var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                      var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                      var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                      string url = $"{_config["AppUrl"]}/api/account/confirmemail?userid={user.Id}&token={validEmailToken}";

                      await _emailService.SendEmailAsync(user.Email, "Confirm Email", $"<h1>Welcome!</h1>" +
                          $"<p>Approve email by clicking <a href='{url}'>this link</a>.</p>");*/

                    responseViewModel.Id = user.Id;
                    responseViewModel.IsSuccess = true;
                    responseViewModel.Message = "User created successfully.";
                }
                else
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = string.Format("An error occurred while creating the user: {0}", result.Errors.FirstOrDefault().Description);
                }

                return Ok(responseViewModel);
            }
            catch (Exception ex)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = ex.Message;

                return BadRequest(responseViewModel);
            }
        }

        // Confirm email controller
        //[HttpGet]
        //[Route("ConfirmEmail")]
        //public async Task<ActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        //        return NotFound();

        //    var result = await _accountService.ConfirmEmailAsync(userId, token);

        //    if (result.IsSuccess)
        //    {
        //        return Ok(result);
        //    }

        //    return BadRequest(result);
        //}


        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            LoginResponse responseViewModel = new();

            try
            {
                #region Validate

                if (ModelState.IsValid == false)
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = "Your information is incomplete, some fields were not submitted. Please fill in all fields.";
                    return BadRequest(responseViewModel);
                }

                //Kulllanıcı bulunur.
                ApplicationUser user = await _userManager.FindByNameAsync(model.Email);

                //Kullanıcı var ise;
                if (user == null)
                {
                    return Unauthorized();
                }

                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user,
                                                                            model.Password,
                                                                            false,
                                                                            false);
                //Kullanıcı adı ve şifre kontrolü
                if (signInResult.Succeeded == false)
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = "Username or password is wrong.";

                    return Unauthorized(responseViewModel);
                }
                #endregion

                ApplicationUser applicationUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);

                AccessTokenGenerator accessTokenGenerator = new(_context, _config, applicationUser);
                ApplicationUserTokens userTokens = accessTokenGenerator.GetToken();
                responseViewModel.Id = user.Id;
                responseViewModel.IsSuccess = true;
                responseViewModel.Message = "User is logged in.";
                responseViewModel.TokenInfo = new TokenInfo
                {
                    Token = userTokens.Value,
                    ExpireDate = userTokens.ExpireDate
                };
                responseViewModel.Role = _userManager.GetRolesAsync(applicationUser).Result.FirstOrDefault();

                return Ok(responseViewModel);
            }
            catch (Exception ex)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = ex.Message;

                return BadRequest(responseViewModel);
            }
        }




        //[HttpPost("ForgetPassword")]
        //public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordViewModel email)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var result = await _accountService.ForgetPasswordAsync(email);

        //        if (result.IsSuccess)
        //            return Ok(result); // 200

        //        return BadRequest(result); // 400
        //    }
        //    return BadRequest("Some properties are not valid");
        //}

        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _accountService.ResetPasswordAsync(model);

        //        if (result.IsSuccess)
        //            return Ok(result);

        //        return BadRequest(result);
        //    }

        //    return BadRequest("Some properties are not valid");
        //}
    }

}
