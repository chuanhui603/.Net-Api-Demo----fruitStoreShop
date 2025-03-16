using Microsoft.AspNetCore.Authorization;
using 水水水果API.DTO.Email;

namespace 水水水果.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(IOptions<AppsetttingModel> options, ILogger<TestController> logger, IMailHelper mailHelper, DownloadHelper downloadHelper,
    IConnectionMultiplexer redis) : ControllerBase
    {
        private ILogger<TestController> _logger = logger;
        private readonly AppsetttingModel _options = options.Value;
        private readonly IMailHelper _mailHelper = mailHelper;
        private readonly DownloadHelper _downloadHelper = downloadHelper;

        private readonly IConnectionMultiplexer _redis = redis;

        // GET: api/<TestController>
        [HttpGet("Log")]
        public void GetLog()
        {

        }

        [HttpGet("Mail")]
        public Task<IActionResult> SendMail()
        {
            _mailHelper.SendEmailiAsync(new MailRequestDTO
            {
                Attachments = null,
                Body = "Test",
                Subject = "Test",
                ToEmail = "chuanhui603@gmail.com"
            }).ConfigureAwait(false);
            return Task.FromResult<IActionResult>(NoContent());
        }

        [HttpGet("Download")]
        public Task<IActionResult> DownloadFile(DataDTO data)
        {
            _downloadHelper.DownloadFile(data);
            return Task.FromResult<IActionResult>(NoContent());
        }


        [HttpGet("test")]
        public void Get()
        {
            var name = User.Identity.Name;
            _logger.LogInformation("TestController Get() called");
            _logger.LogInformation("Test Identity : {name}", name);
            _logger.LogInformation("_options Value: " + _options.testValue);
        }

        [Authorize]
        [HttpGet("claim")]
        public ActionResult Claim()
        {
            var user = HttpContext.User;
            var type = HttpContext.GetType();
            _logger.LogInformation("Test Identity : {user}", user);
            _logger.LogInformation("Test Identity : {Type}", type);
            _logger.LogInformation("Test Identity : {Value}", user.Identity.Name);
            var memberId = user.Claims.FirstOrDefault((c) => c.Type == "MemberId");
            _logger.LogInformation("MemberGuid: {id}", memberId.Value);
            return Ok(user.Claims.Select(p => new { p.Type, p.Value }));
        }

        [HttpGet("redis")]
        public ActionResult Redis()
        {
            var db = _redis.GetDatabase();
            var pong = db.Ping();
            _logger.LogInformation("Ping to Redis: {pong}", pong);
            return Ok($"Ping to Redis: {pong}");
        }
    }

}
