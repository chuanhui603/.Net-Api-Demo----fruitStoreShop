using Microsoft.AspNetCore.Authorization;

namespace 水水水果.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly ILogger<RecipientController> _logger;
        private readonly IRecipientService _recipientService;


        public RecipientController(ILogger<RecipientController>logger, IRecipientService recipientService)
        {
            _logger = logger;
            _recipientService = recipientService;
        }

        // GET: api/<RecipientController>
        [HttpGet]
        public IActionResult Get()
        {
            var user = HttpContext.User;
            user.Identities.Count();
            var type = HttpContext.GetType();
            _logger.LogInformation("Test Identity : {user}", user.Identities.Count());
            _logger.LogInformation("Test Identity : {Type}", type.Name);
            _logger.LogInformation("Test Identity : {Value}", user.Identity.Name);
            var recipients = _recipientService.GetRecipients();
            return Ok(recipients);
        }

        // GET api/<RecipientController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
       
            var recipient = _recipientService.GetRecipientById(id);
            if (recipient == null)
            {
                return NotFound();
            }
            return Ok(recipient);
        }

        // POST api/<RecipientController>
        [HttpPost]
        public IActionResult Post([FromBody] RecipientDTO recipient)
        {
            _recipientService.CreateRecipient(recipient);
            return Created();
        }

        // PUT api/<RecipientController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] RecipientDTO recipient)
        {
            if (id != recipient.Id)
            {
                return BadRequest();
            }
            _recipientService.UpdateRecipient(id, recipient);
            return NoContent();
        }

        // DELETE api/<RecipientController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _recipientService.DeleteRecipient(id);
            return NoContent();
        }
    }
}
