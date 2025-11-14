using 水水水果API.Models.DTO;

namespace 水水水果API.Services
{
    internal class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ILogger<CouponService> _logger;
        public CouponService(ILogger<CouponService> logger, ICouponRepository couponRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _couponRepository = couponRepository;
            _httpcontext = httpContextAccessor;
        }
        public IEnumerable<CouponDTO> GetCoupons()
        {
            return _couponRepository.GetCoupons().Select(coupon => new CouponDTO
            {
           
            });
        }
        public IEnumerable<CouponDTO> GetCouponByPage(int page, int pageSize)
        {
            var result = _couponRepository.GetCouponsByPage(page, pageSize).Select(coupon => new CouponDTO
            {
              
            });

            return result;
        }
        public CouponDTO GetCouponByCode(string Code, int id)
        {
            _logger.LogInformation("Start GetCouponByCode");
            var result = _couponRepository.GetCouponByCode(Code, id);
            _logger.LogInformation("result: {0}", _logger);
            return new CouponDTO
            {
              
            };
        }


        public CouponDTO GetCouponById(int id)
        {
            var coupon = _couponRepository.GetCouponById(id);
            return new CouponDTO
            {
            
            };
        }

        public void CreateCoupon(CouponDTO coupon)
        {
            _couponRepository.CreateCoupon(new Coupon
            {
              
            });
        }

        public void RegisterMemberCode(string Code, int id)
        {
            _logger.LogInformation("Start Register");
            var result = _couponRepository.GetCouponByCode(Code, id);
            if (result == null) return;
            _logger.LogInformation("result: {0}", result);
            var user = _httpcontext.HttpContext.User;
            var memberIdClaim = user.Claims.FirstOrDefault(c => c.Type == "Email");
            _logger.LogInformation("memberIdClaim: {0}", memberIdClaim);
            _couponRepository.RegiserCoupon(new CustomerCoupon
            {
           
            });
        }

        public void UpdateCoupon(CouponDTO coupon)
        {
            _logger.LogInformation("Start Update");
            var Id = coupon.Id!.Value;
            _couponRepository.UpdateCoupon(new Coupon
            {
           
            });
        }

        public void DeleteCoupon(int id)
        {
            _couponRepository.DeleteCoupon(id);
        }


    }
}
