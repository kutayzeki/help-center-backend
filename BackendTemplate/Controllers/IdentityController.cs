using BackendTemplate.Core.Helpers.ResponseModels;
using BackendTemplate.Core.Services.MailService;
using BackendTemplate.Core.Utilities;
using BackendTemplate.Models;
using BackendTemplate.Models.User;
using Ekip.Core.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendTemplate.Controllers
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
                    responseViewModel.Message = "Bilgileriniz eksik, bazı alanlar gönderilmemiş. Lütfen tüm alanları doldurunuz.";

                    return BadRequest(responseViewModel);
                }

                ApplicationUser existsUser = await _userManager.FindByNameAsync(model.Email);

                if (existsUser != null)
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = "Kullanıcı zaten var.";

                    return BadRequest(responseViewModel);
                }
                #endregion

                //Kullanıcı bilgileri set edilir.
                ApplicationUser user = new()
                {
                    Email = model.Email.Trim(),
                    UserName = model.Email.Trim()
                };

                //Kullanıcı oluşturulur.
                IdentityResult result = await _userManager.CreateAsync(user, model.Password.Trim());

                //Kullanıcı oluşturuldu ise  
                if (result.Succeeded)
                {
                    //rol içerde yoksa oluşturuluyor.
                    bool roleExists = await _roleManager.RoleExistsAsync(_config[$"Roles:{model.Role}"]);
                    if (!roleExists)
                    {
                        IdentityRole role = new(_config[$"Roles:{model.Role}"])
                        {
                            NormalizedName = _config[$"Roles:{model.Role}"]
                        };
                        _roleManager.CreateAsync(role).Wait();
                    }

                    //Kullanıcıya ilgili rol ataması yapılır.
                    //TODO role bulunamazsa kullanıcı rolsüz oluşuyor. Edge case olabilir.
                    _userManager.AddToRoleAsync(user, _config["Roles:" + model.Role]).Wait();

                    //Send email
                    /*  var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                      var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                      var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                      string url = $"{_config["AppUrl"]}/api/account/confirmemail?userid={user.Id}&token={validEmailToken}";

                      await _emailService.SendEmailAsync(user.Email, "Emailinizi onaylayın", $"<h1>Sınıf'a hoş geldin!</h1>" +
                          $"<p>Lütfen emailinizi <a href='{url}'>bu linke</a> tıklayarak onaylayın</p>");*/

                    responseViewModel.Id = user.Id;
                    responseViewModel.IsSuccess = true;
                    responseViewModel.Message = "Kullanıcı başarılı şekilde oluşturuldu.";
                }
                else
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = string.Format("Kullanıcı oluşturulurken bir hata oluştu: {0}", result.Errors.FirstOrDefault().Description);
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
                    responseViewModel.Message = "Bilgileriniz eksik, bazı alanlar gönderilmemiş. Lütfen tüm alanları doldurunuz.";
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
                    responseViewModel.Message = "Kullanıcı adı veya şifre hatalı.";

                    return Unauthorized(responseViewModel);
                }
                #endregion

                ApplicationUser applicationUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
                var userRoleId = _context.UserRoles.FirstOrDefault(x => x.UserId == applicationUser.Id);
                var roles = _context.Roles.FirstOrDefault(x => x.Id == userRoleId.RoleId);

                AccessTokenGenerator accessTokenGenerator = new(_context, _config, applicationUser);
                ApplicationUserTokens userTokens = accessTokenGenerator.GetToken();
                responseViewModel.Id = user.Id;
                responseViewModel.IsSuccess = true;
                responseViewModel.Message = "Kullanıcı giriş yaptı.";
                responseViewModel.TokenInfo = new TokenInfo
                {
                    Token = userTokens.Value,
                    ExpireDate = userTokens.ExpireDate
                };

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
