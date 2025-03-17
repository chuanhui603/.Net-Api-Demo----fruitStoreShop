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
                Id = coupon.Id,
                ExpiriedDate = coupon.ExpiriedDate,
                DiscountAmount = coupon.DiscountAmount,
                Code = coupon.Code,
            });
        }
        public IEnumerable<CouponDTO> GetCouponByPage(int page, int pageSize)
        {
            var result = _couponRepository.GetCouponsByPage(page, pageSize).Select(coupon => new CouponDTO
            {
                Id = coupon.Id,
                Name = coupon.Name,
                IsActive = coupon.IsActive,
                ExpiriedDate = coupon.ExpiriedDate,
                DiscountAmount = coupon.DiscountAmount,
                Code = coupon.Code,
            });

            return result;
        }
        public CouponDTO GetCouponByCode(string Code, Guid id)
        {
            _logger.LogInformation("Start GetCouponByCode");
            var result = _couponRepository.GetCouponByCode(Code, id);
            _logger.LogInformation("result: {0}", _logger);
            return new CouponDTO
            {
                Id = result.Id,
                Code = result.Code,
                Name = result.Name,
                ExpiriedDate = result.ExpiriedDate.Value,
                IsActive = result.IsActive.Value,
                DiscountAmount = result.DiscountAmount
            };
        }


        public CouponDTO GetCouponById(Guid id)
        {
            var coupon = _couponRepository.GetCouponById(id);
            return new CouponDTO
            {
                Id = coupon.Id,
                ExpiriedDate = coupon.ExpiriedDate.Value,
                DiscountAmount = coupon.DiscountAmount.Value,
                Code = coupon.Code,
            };
        }

        public void CreateCoupon(CouponDTO coupon)
        {
            _couponRepository.CreateCoupon(new Coupon
            {
                Id = Guid.NewGuid(),
                ExpiriedDate = coupon.ExpiriedDate,
                DiscountAmount = coupon.DiscountAmount,
                Code = coupon.Code,
                Name = coupon.Name,
                IsActive = true,
            });
        }

        public void RegisterMemberCode(string Code, Guid id)
        {
            _logger.LogInformation("Start Register");
            var result = _couponRepository.GetCouponByCode(Code, id);
            if (result == null) return;
            _logger.LogInformation("result: {0}", result);
            var user = _httpcontext.HttpContext.User;
            var memberIdClaim = user.Claims.FirstOrDefault(c => c.Type == "Email");
            _logger.LogInformation("memberIdClaim: {0}", memberIdClaim);
            _couponRepository.RegiserCoupon(new MemberCoupon
            {
                Id = Guid.NewGuid(),
                MemberId = new Guid(memberIdClaim.Value),
                CouponId = result.Id,
                IsUsed = true,
                ExpiredAt = result.ExpiriedDate.Value,
                IssuedAt = DateTime.Now
            });
        }

        public void UpdateCoupon(CouponDTO coupon)
        {
            _logger.LogInformation("Start Update");
            var Id = coupon.Id!.Value;
            _couponRepository.UpdateCoupon(new Coupon
            {
                Id = Id,
                ExpiriedDate = coupon.ExpiriedDate,
                DiscountAmount = coupon.DiscountAmount,
                Code = coupon.Code,
                Name = coupon.Name,
                IsActive = coupon.IsActive,
            });
        }

        public void DeleteCoupon(Guid id)
        {
            _couponRepository.DeleteCoupon(id);
        }


    }
}
