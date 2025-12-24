using Azure.Core;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMemberApplicationService _memberRegistrationOrchestrator;

        public CustomerController(
            ILogger<CustomerController> logger,
            IMemberService memberService,
             IAuthService authService,
            IMemberApplicationService memberRegistrationOrchestrator)
        {
            _logger = logger;
            _memberService = memberService;
            _authService = authService;
            _memberRegistrationOrchestrator = memberRegistrationOrchestrator;
        }

        // GET: api/<CustomerController>/Info
        [HttpPost("Info")]
        public IActionResult Get(List<int> customers)
        {
            try
            {
                var members = _memberService.GetMembers(customers);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var member = _memberService.GetMemberById(id);
                if (member == null)
                {
                    return NotFound();
                }
                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Member")]
        public IActionResult CreateMember([FromBody] MemberCreate memberCreate)
        {
            try
            {
                var memberId = _memberRegistrationOrchestrator.RegisterMember(memberCreate);

            
                return Ok(new { MemberId = memberId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "會員註冊失敗");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("Member")]
        public IActionResult UpdateMember([FromBody] MemberUpdate memberUpdate)
        {
            try
            {
                var memberId = _memberService.UpdateMember(memberUpdate);
                return Ok(new { MemberId = memberId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "會員更新失敗");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public IActionResult MemberDelete(int id)
        {
            try
            {
                _memberService.DeleteMember(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
