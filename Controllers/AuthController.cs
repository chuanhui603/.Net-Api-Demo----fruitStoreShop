using Microsoft.AspNetCore.Authorization;
namespace 水水水果API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginDTO login)
    {
        try
        {
            LoginResponseDTO result = _authService.Login(login);
            if (result == null) return BadRequest("帳號登入錯誤");
            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }

    }

    [Authorize]
    [ServiceFilter(typeof(LogoutActionFilter))]
    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        try
        {
            _authService.Logout();
            return Ok("Success Log Out");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost("Refresh")]
    public IActionResult RefreshToken(string refreshToken)
    {
        try
        {
            var newRefreshToken = _authService.RefreshToken(refreshToken);
            if (newRefreshToken == null)
            {
                return NotFound(false);
            }
            return Ok(newRefreshToken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}

