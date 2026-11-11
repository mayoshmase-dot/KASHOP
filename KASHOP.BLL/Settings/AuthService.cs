using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL.Settings
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplictionUser> _userManager;

        public AuthService(UserManager<ApplictionUser> userManager )
        {
            _userManager = userManager;
        }
        public async Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplictionUser>();
            var result = await _userManager.CreateAsync(user , request.Password);
            if (!result.Succeeded)
            {
                return (false ,result.Errors.Select(e=>e.Description).ToList());
            }
            await _userManager.AddToRoleAsync(user, "User");
            return (true , []);
        }
    }
}
