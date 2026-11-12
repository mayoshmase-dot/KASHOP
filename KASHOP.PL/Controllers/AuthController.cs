using KASHOP.BLL;
using KASHOP.BLL.Settings;
using KASHOP.DAL.DTO.Request;
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
            var (success, errors) = await _authService.RegisterAsync(request);

            if (!success)
                return BadRequest(new { errors });

            return Ok(new { message = "User registered successfully" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var (success, errors , response) = await _authService.LoginAsync(request);

            if (!success)
                return BadRequest(new { errors });

            return Ok(response);
        }
    }
}
