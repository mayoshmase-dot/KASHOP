using KASHOP.BLL.Common;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responses;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace KASHOP.BLL.Settings
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplictionUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<ApplictionUser> _signInManager;

        public AuthService(UserManager<ApplictionUser> userManager, IConfiguration configuration , ITokenService tokenService,
           SignInManager<ApplictionUser> signInManager, IEmailService emailService
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _emailService = emailService;
            _signInManager = signInManager;
        }
        public async Task<Result< bool >> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplictionUser>();
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Result<bool>.Validation(result.Errors.Select(e => e.Description).ToList());
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var anCodedToken = Uri.EscapeDataString(token);
            var link = $"https://localhost:7213/api/auth/confirm-email?userId={user.Id}&token={anCodedToken}";
            await _emailService.SendEmailAsync(user.Email, "Confirm Your Email" , " " + $"<div><h2>Welcome {user.UserName}</h2>" +
            "<p>Please confirm your email by clicking the link below</p>" +
            $"<a href={link}>Confirm Email</a></div>");
            await _userManager.AddToRoleAsync(user, "User");
            return Result<bool>.Success(true);
        }

        public async Task<Result<LoginResponses>> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result<LoginResponses>.UnAuthorized(["invaid email or password"]);
         
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (result.IsNotAllowed)
            {
                return Result<LoginResponses>.Forbidden(["confirm your email"]);
            }

            if (result.IsLockedOut)
                {
                return Result<LoginResponses>.Forbidden(["Account is locked , try again alter"]);
                }
            if (!result.Succeeded)
            {
                return Result<LoginResponses>.UnAuthorized(["invaid email or password"]);
            }

            var token = await _tokenService.GenerateJwt(user);
            return Result<LoginResponses>.Success(new LoginResponses { Token = token});
        }

        public async Task<Result<bool>> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<bool>.Validation(["invaid confirmation link"]);

            if (user.EmailConfirmed)
                return Result<bool>.Success(true);

            var result = await _userManager.ConfirmEmailAsync(user , token);
            return result.Succeeded ? Result<bool>.Success(true) : Result<bool>.Validation([""]) ; 
        }

    }
};