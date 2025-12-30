using Azure.Core;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Authorization;
using Mysqlx;
using MySqlX.XDevAPI.Common;
using Serilog;
using 水水水果API.Models.CRM;
using 水水水果API.Models.DTO;

namespace 水水水果.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogoutActionFilter))]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMemberService _memberService;
        private readonly IAuthService _authService;
        private readonly IMemberApplicationService _memberApplicationService;

        public CustomerController(
            ILogger<CustomerController> logger,
            IMemberService memberService,
             IAuthService authService,
            IMemberApplicationService memberApplicationService)
        {
            _logger = logger;
            _memberService = memberService;
            _authService = authService;
            _memberApplicationService = memberApplicationService;
        }

        // GET api/<CustomerController>/email
        [HttpGet("Member")]
        public IActionResult GetByAccount([FromQuery] string account)
        {
            try
            {
                User user = _authService.GetUserByEmail(account);
                if (user == null) return NotFound();
                MemberResponse member = _memberService.GetMemberByUser(user);
                if (member == null)
                    return NotFound();
                return Ok(ApiResponse<MemberResponse>.Ok(member));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Search Error");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET api/<CustomerController>/exist
        [AllowAnonymous]
        [HttpGet("Exists")]
        public IActionResult ValidateMember([FromQuery] string email)
        {
            try
            {
                var exist = _authService.ValidMemberByEmail(email);
                return Ok(ApiResponse<bool>.Ok(exist, exist ? "該會員已被使用" : "該會員可以使用"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validate Error");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Member")]
        public IActionResult CreateMember([FromBody] MemberCreate memberCreate)
        {
            try
            {
                MemberResponse member = _memberApplicationService.RegisterMember(memberCreate);
                LoginResponseDTO result = _authService.Login(new LoginDTO()
                {
                    Email = memberCreate.Email,
                    Password = memberCreate.PassWord,
                });
                result.User = member;
                return Ok(ApiResponse<LoginResponseDTO>.Ok(result));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register Error");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("Member")]
        public IActionResult UpdateMember([FromBody] MemberUpdate memberUpdate)
        {
            try
            {
                _memberApplicationService.UpdateMember(memberUpdate);
                return Ok(ApiResponse<bool>.Ok(true, "Update Success"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Error");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{email}")]

        public IActionResult MemberDelete(string email)
        {
            try
            {
                User user = _authService.GetUserByEmail(email);
                if (user == null)
                    return NotFound();
                _memberService.DeleteMember(user.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Error");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
