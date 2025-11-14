namespace 水水水果API.Interfaces
{
    public interface ICouponRepository
    {
        IEnumerable<Coupon> GetCoupons();
        IEnumerable<Coupon> GetCouponsByPage(int page, int pageSize);
        Coupon GetCouponById(int id);
        Coupon GetCouponByCode(string Code, int id);
        void CreateCoupon(Coupon coupon);
        void RegiserCoupon(CustomerCoupon couponId);
        void UpdateCoupon(Coupon coupon);
        void DeleteCoupon(int id);
    }
}