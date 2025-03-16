using 水水水果API.DTO.LinePay;

namespace 水水水果API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayMentController : ControllerBase
    {
        private readonly ILogger<PayMentController> _logger;
        private readonly ILinePayService _linePayService;
        public PayMentController(ILogger<PayMentController> logger, ILinePayService linePayService)
        {
            this._logger = logger;
            _linePayService = linePayService;
        }

        [HttpPost("LinePay/Create")]
        public async Task<IActionResult> LinePayRequest([FromBody] LinePayRequestDTO dto)
        {
            var result = await _linePayService.SendPaymentRequest(dto);
            _logger.LogInformation("DTO: {dto}", dto);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }


        [HttpPost("LinePay/Confirm")]
        public async Task<IActionResult> LinePayConfirmRequest([FromQuery] string transactionId, [FromQuery] string orderId, LinePayConfirmDTO dto)
        {
            var result = await _linePayService.ConfirmPayment(transactionId, orderId,dto);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("LinePay/Cancel")]
        public async Task<IActionResult> CancelTransaction([FromQuery] string transactionId)
        {
            return NoContent();
        }
    }
}
