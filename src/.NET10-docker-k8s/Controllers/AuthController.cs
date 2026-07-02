using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Net10.docker.k8s.DTO.Auth;
using Net10.docker.k8s.Model;
using Net10.docker.k8s.Services.Impl;
using Net10.docker.k8s.Services;
using Microsoft.AspNetCore.Identity;

namespace Net10.docker.k8s.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(AuthService authService, IUserService userService, IPasswordHasher<User> passwordHasher)
        {
            _authService = authService;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginRequestDto dto)
        {
            if (_userService.GetByUsername(dto.Username) != null) return Conflict("User already exists");

            var user = new User
            {
                Username = dto.Username
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _userService.Create(user);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto dto)
        {
            var result = _authService.Authenticate(dto.Username, dto.Password);
            if (!result.Success) return Unauthorized();
            return Ok(new AuthResponseDto { AccessToken = result.AccessToken!, RefreshToken = result.RefreshToken!, ExpiresIn = result.ExpiresIn });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] AuthResponseDto dto)
        {
            // Refresh expects AccessToken to be ignored and provide Username via claims; for simplicity expect username in body as AccessToken
            // In real scenario, client should send refresh token and a way to identify user; here we decode username from request headers or include username
            var username = Request.Headers["X-Username"].ToString();
            if (string.IsNullOrEmpty(username)) return BadRequest("Missing X-Username header for refresh");

            var res = _authService.Refresh(username, dto.RefreshToken);
            if (!res.Success) return Unauthorized();
            return Ok(new AuthResponseDto { AccessToken = res.AccessToken!, RefreshToken = res.RefreshToken!, ExpiresIn = 0 });
        }

        [Authorize]
        [HttpPost("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return BadRequest();
            var ok = _authService.Revoke(username);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
