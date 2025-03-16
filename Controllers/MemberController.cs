namespace 水水水果.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IMemberService _memberService;

        public MemberController(ILogger<MemberController> logger, IMemberService memberService)
        {
            _logger = logger;
            _memberService = memberService;
        }

        // GET: api/<MemberController>
        [HttpGet]
        public IActionResult Get()
        {
            var members = _memberService.GetMembers();
            return Ok(members);
        }

        // GET api/<MemberController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var member = _memberService.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        // POST api/<MemberController>
        [HttpPost]
        public IActionResult Post([FromBody] MemberDTO member)
        {
             _logger.LogInformation("member: {member}", member);
            _memberService.CreateMember(member);
            return Created();
        }

        // PUT api/<MemberController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] MemberDTO member)
        {
           
            if (id != member.Id)
            {
                return BadRequest();
            }
            _memberService.UpdateMember(id, member);
            return NoContent();
        }

        // DELETE api/<MemberController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _memberService.DeleteMember(id);
            return NoContent();
        }
    }
}
