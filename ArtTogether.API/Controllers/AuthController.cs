using ArtTogether.Application.DTOs;
using ArtTogether.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtTogether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("guest-signin")]
        public async Task<IActionResult> AnonymousLogin()
        {
            var result = await authService.LoginAsGuestAsync();
            return Ok(result);
        }

        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignin([FromBody] GoogleSigninDto dto)
        {
            var result = await authService.LoginWithGoogleAsync(dto);
            return Ok(result);
        }
    }
}
