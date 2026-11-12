using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KASHOP.BLL.Settings
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplictionUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenService(UserManager<ApplictionUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> GenerateJwt(ApplictionUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.NameIdentifier , user.Id),
                new Claim (ClaimTypes.Name , user.UserName),
                new Claim (ClaimTypes.Email, user.Email),
                new Claim (ClaimTypes.Role , string.Join(" , " , roles))

            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
           issuer: _configuration["Jwt:Issuer"],
           audience: _configuration["Jwt:Audience"],
           claims: userClaims,
           expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
           signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
       );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
