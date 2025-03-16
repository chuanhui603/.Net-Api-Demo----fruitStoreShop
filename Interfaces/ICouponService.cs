namespace 水水水果API.Interfaces
{
    public interface ICouponService
    {
        IEnumerable<CouponDTO> GetCoupons();
        IEnumerable<CouponDTO> GetCouponByPage(int page, int pageSize);
        CouponDTO GetCouponById(Guid id);
        CouponDTO GetCouponByCode(string Code, Guid id);
        void CreateCoupon(CouponDTO coupon);
        void RegisterMemberCode(string Code, Guid id);
        void UpdateCoupon(CouponDTO coupon);
        void DeleteCoupon(Guid id);
    }
}
