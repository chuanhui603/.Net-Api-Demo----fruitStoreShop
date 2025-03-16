namespace 水水水果API.Interfaces
{
    public interface ICouponRepository
    {
        IEnumerable<Coupon> GetCoupons();
        IEnumerable<Coupon> GetCouponsByPage(int page, int pageSize);
        Coupon GetCouponById(Guid id);
        Coupon GetCouponByCode(string Code, Guid id);
        void CreateCoupon(Coupon coupon);
        void RegiserCoupon(MemberCoupon couponId);
        void UpdateCoupon(Coupon coupon);
        void DeleteCoupon(Guid id);
    }
}