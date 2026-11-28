using KASHOP.BLL;
using KASHOP.BLL.Common;
using KASHOP.BLL.Settings;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Extenstions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
      
        [HttpPost("register")]
        public async Task<IActionResult> Create(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RegisterAsync(request);

            return result.IsSuccess ? Ok() : result.ToErrorResponse();

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(request);

            return result.IsSuccess ? Ok() : result.ToErrorResponse();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId , [FromQuery] string token )
        {
            var result = await _authService.ConfirmEmailAsync(userId,token);

            return result.IsSuccess ? Ok() : result.ToErrorResponse();
        }
    }
}
