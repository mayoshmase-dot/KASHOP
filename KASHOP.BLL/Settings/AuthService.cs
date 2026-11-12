using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KASHOP.BLL.Settings
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplictionUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplictionUser> _signInManager;

        public AuthService(UserManager<ApplictionUser> userManager, IConfiguration configuration , ITokenService tokenService,
           SignInManager<ApplictionUser> signInManager
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        public async Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplictionUser>();
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToList());
            }
            await _userManager.AddToRoleAsync(user, "User");
            return (true, []);
        }

        public async Task<(bool Success, List<string> Errors, LoginResponses response)> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
              return (false , ["invaid email or password"] , null);
         
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (result.IsNotAllowed)
            {
                return (false, ["confirm your email"], null);

            }

            if (result.IsLockedOut)
                {
                    return (false, ["Account is locked , try again alter"], null);
                }
            if (!result.Succeeded)
            {
                return (false, ["invaid email or password"], null);
            }

            var token = await _tokenService.GenerateJwt(user);
            var response = new LoginResponses()
            {
                Token = token

            };
            return (true , [] , response);
        }


    }
};