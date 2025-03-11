

using ApplicationLayer.Dtos;
using ApplicationLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApis.Controllers
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
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterUser(request);
            if (response.RespCode == 200)
                return Ok(response);
            else
                return StatusCode(response.RespCode, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.AuthenticateUser(request.Email, request.Password);
            if (response.RespCode == 200)
                return Ok(response);
            else
                return StatusCode(response.RespCode, response);
        }
    }
}
