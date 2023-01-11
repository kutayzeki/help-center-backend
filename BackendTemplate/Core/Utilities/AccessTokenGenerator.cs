using BackendTemplate.Models.User;
using BackendTemplate.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackendTemplate.Core.Utilities
{
    public class AccessTokenGenerator
    {
        public APIDbContext _context { get; set; }
        public IConfiguration _config { get; set; }
        public ApplicationUser _applicationUser { get; set; }


        public AccessTokenGenerator(APIDbContext context,
                                    IConfiguration config,
                                    ApplicationUser applicationUser)
        {
            _config = config;
            _context = context;
            _applicationUser = applicationUser;
        }


        public ApplicationUserTokens GetToken()
        {
            ApplicationUserTokens userTokens = null;
            TokenInfo tokenInfo = null;

            //Check if the user has a pre-created token.
            if (_context.ApplicationUserTokens.Count(x => x.UserId == _applicationUser.Id) > 0)
            {
                // Relevant token information is found.
                userTokens = _context.ApplicationUserTokens.FirstOrDefault(x => x.UserId == _applicationUser.Id);

                // If it is expired, it creates a new token and updates it.
                if (userTokens.ExpireDate <= DateTime.Now)
                {
                    //Create new token
                    tokenInfo = GenerateToken();

                    userTokens.ExpireDate = tokenInfo.ExpireDate;
                    userTokens.Value = tokenInfo.Token;

                    _context.ApplicationUserTokens.Update(userTokens);
                }
            }
            else
            {
                //Create new token
                tokenInfo = GenerateToken();

                userTokens = new ApplicationUserTokens();

                userTokens.UserId = _applicationUser.Id;
                userTokens.LoginProvider = "SystemAPI";
                userTokens.Name = _applicationUser.UserName;
                userTokens.ExpireDate = tokenInfo.ExpireDate;
                userTokens.Value = tokenInfo.Token;

                _context.ApplicationUserTokens.Add(userTokens);
            }

            _context.SaveChangesAsync();

            return userTokens;
        }

        public async Task<bool> DeleteToken()
        {
            bool ret = true;

            try
            {
                //Check if the user has a pre-created token.
                if (_context.ApplicationUserTokens.Count(x => x.UserId == _applicationUser.Id) > 0)
                {
                    ApplicationUserTokens userTokens = userTokens = _context.ApplicationUserTokens.FirstOrDefault(x => x.UserId == _applicationUser.Id);

                    _context.ApplicationUserTokens.Remove(userTokens);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }

       
        private TokenInfo GenerateToken()
        {
            DateTime expireDate = DateTime.Now.AddDays(50);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Application:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _config["Application:Audience"],
                Issuer = _config["Application:Issuer"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    // Claim definitions are made. The most important here are the Id and email.
                    // Via Id, we will find the active user.
                    new Claim(ClaimTypes.NameIdentifier, _applicationUser.Id),
                    new Claim(ClaimTypes.Name, _applicationUser.UserName),
                    new Claim(ClaimTypes.Email, _applicationUser.Email)
                }),

                //ExpireDate
                Expires = expireDate,

                //specify the encryption type: HmacSha256Signature
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            TokenInfo tokenInfo = new TokenInfo();
            
            tokenInfo.Token = tokenString;
            tokenInfo.ExpireDate = expireDate;

            return tokenInfo;
        }
    }
}
