using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface ICouponService
    {
        IEnumerable<CouponDTO> GetCoupons();
        IEnumerable<CouponDTO> GetCouponByPage(int page, int pageSize);
        CouponDTO GetCouponById(int id);
        CouponDTO GetCouponByCode(string Code, int id);
        void CreateCoupon(CouponDTO coupon);
        void RegisterMemberCode(string Code, int id);
        void UpdateCoupon(CouponDTO coupon);
        void DeleteCoupon(int id);
    }
}
