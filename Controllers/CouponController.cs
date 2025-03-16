using Microsoft.AspNetCore.Authorization;

namespace 水水水果.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogoutActionFilter))]
    [ApiController]
    [Authorize]
    public class CouponController(ILogger<CouponController> logger, ICouponService couponService, ICouponRepository couponRepository) : ControllerBase
    {
        private readonly ICouponService _couponService = couponService;
        private readonly ICouponRepository _couponRepository = couponRepository;
        private readonly ILogger<CouponController> _logger = logger;

        // GET: api/<CouponController>
        [HttpGet]
        public IActionResult Get()
        {
            var coupons = _couponService.GetCoupons();
            return Ok(coupons);
        }

        // GET api/<CouponController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var coupon = _couponService.GetCouponById(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }

        [HttpGet("page={page}/top={pageSize}")]
        public ActionResult<List<CouponDTO>> GetByPage([FromRoute] int page, [FromRoute] int pageSize)
        {
            try
            {
                _logger.LogInformation("page:{page} pageSize:{pageSize}", page, pageSize);
                var coupons = _couponService.GetCouponByPage(page, pageSize).ToList();
                return Ok(coupons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting coupons by page");
                return StatusCode(500, new { message = "An error occurred while getting the coupons by page." });
            }

        }


        // POST api/<CouponController>
        [HttpPost("create")]
        public IActionResult Post([FromBody] CouponDTO coupon)
        {
            _couponService.CreateCoupon(coupon);
            return Created();
        }


        [HttpPost("resgister")]
        public IActionResult MemberPost([FromBody] CouponDTO couponDTO)
        {
            _logger.LogInformation("Code: {0}", couponDTO.Code);
            try
            {
                var result = _couponRepository.GetCouponByCode(couponDTO.Code, couponDTO.Id!.Value);
                if (result == null) return StatusCode(200, new { message = "No Code" });
                _logger.LogInformation("result: {0}", result);
                _couponService.RegisterMemberCode(couponDTO.Code, couponDTO.Id.Value);
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering member code");
                return StatusCode(500, new { message = "An error occurred while registering the member code." });
            }
        }

        // PUT api/<CouponController>/5
        [HttpPut("update")]
        public IActionResult Put([FromBody] CouponDTO coupon)
        {
            if (coupon.Id == null)
            {
                return BadRequest();
            }
            _logger.LogInformation("Start");
            _couponService.UpdateCoupon(coupon);
            return NoContent();
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _couponService.DeleteCoupon(id);
            return NoContent();
        }
    }
}
