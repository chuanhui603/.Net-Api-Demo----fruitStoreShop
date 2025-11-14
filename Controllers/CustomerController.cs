using Microsoft.AspNetCore.Authorization;

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

        public CustomerController(ILogger<CustomerController> logger, IMemberService memberService)
        {
            _logger = logger;
            _memberService = memberService;
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

        // POST api/<CustomerController>
        [HttpPost("Create")]
        [AllowAnonymous]
        public IActionResult Post([FromBody] UserCreate member)
        {
            try
            {
                _logger.LogInformation("member: {member}", member);
                _memberService.CreateMember(member);
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // PUT api/<CustomerController>
        [HttpPut]
        public IActionResult Put([FromBody] UserUpdate member)
        {
            try
            {
                _memberService.UpdateMember(member);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _memberService.DeleteCustomer(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
