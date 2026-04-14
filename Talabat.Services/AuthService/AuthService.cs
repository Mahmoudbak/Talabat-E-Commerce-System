using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Identity;
using Talabat.Core.Services.Content;

namespace Talabat.Application.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            #region Information Extchange
            //privat Claim (User-Defined)
            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.DispalyName),
                new Claim(ClaimTypes.Email,user.Email)

            };
            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }
            #endregion
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"]?? string.Empty));



            var Token = new JwtSecurityToken
                (
                    audience: _configuration["JWT:ValidAudience"],
                    issuer: _configuration["JWT:ValidIssuer"],
                    expires: DateTime.Now.AddDays(double.Parse( _configuration["JWT:DurationInDays"]?? "0")),
                    claims:authClaim ,
                    signingCredentials:new SigningCredentials(authkey,SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);

        }


    }
}
