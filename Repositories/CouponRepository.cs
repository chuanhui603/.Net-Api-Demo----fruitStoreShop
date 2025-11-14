using Framework.SqlCommon.SQLHelper;
using Microsoft.EntityFrameworkCore;

namespace 水水水果API.Repositories
{
    internal class CouponRepository : ICouponRepository
    {
        private readonly TWRMS_TESTContext _rmsConnection;
        private readonly SqlCommonContext _commonContext;
        private readonly ILogger<CouponRepository> _logger;

        public CouponRepository(ILogger<CouponRepository> logger, TWRMS_TESTContext rmsConnection, SqlCommonContext commonContext)
        {
            _rmsConnection = rmsConnection;
            _commonContext = commonContext;
            _logger = logger;
        }

        public IEnumerable<Coupon> GetCoupons()
        {
            return [.. _rmsConnection.Coupons];
        }
        public IEnumerable<Coupon> GetCouponsByPage(int page, int pageSize)
        {
            return [.. _rmsConnection.Coupons.Skip(page).Take(pageSize)];
        }
        public Coupon GetCouponById(int id)
        {
            return _rmsConnection.Coupons.Single(x=>x.Id == id);
        }

        public Coupon GetCouponByCode(string Code, int id)
        {
            var sql = $"SELECT id,code,discount_amount,name,expirieddate FROM {_commonContext.RMS.Coupon}  WHERE code = @code And id = @id";
            return _rmsConnection.Coupons.FromSqlRaw(sql, new { Code, id }).Single();
        }

        public void CreateCoupon(Coupon coupon)
        {
            _rmsConnection.Coupons.Add(coupon);
            _rmsConnection.SaveChanges();
        }

        public void RegiserCoupon(CustomerCoupon customerCoupon)
        {
        
        }
        public void UpdateCoupon(Coupon coupon)
        {
            _rmsConnection.Coupons.Update(coupon);
            _rmsConnection.SaveChanges();
        }

        public void DeleteCoupon(int id)
        {
            var coupon = _rmsConnection.Coupons.Single(x => x.Id == id);
            _rmsConnection.Coupons.Remove(coupon);
            _rmsConnection.SaveChanges();
        }
    }
}