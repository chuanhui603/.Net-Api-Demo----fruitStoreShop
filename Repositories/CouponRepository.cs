namespace 水水水果API.Repositories
{
    internal class CouponRepository : ICouponRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _tableName = "coupon";
        private readonly ILogger<CouponRepository> _logger;

        public CouponRepository(ILogger<CouponRepository> logger, IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public IEnumerable<Coupon> GetCoupons()
        {
            return _dbConnection.GetAll<Coupon>(_tableName);
        }
        public IEnumerable<Coupon> GetCouponsByPage(int page, int pageSize)
        {
          return _dbConnection.GetByPage<Coupon>(_tableName, page, pageSize);
        }
        public Coupon GetCouponById(Guid id)
        {
            return _dbConnection.GetById<Coupon>(_tableName, id);
        }

        public Coupon GetCouponByCode(string Code, Guid id)
        {
            var sql = "SELECT id,code,discount_amount,name,expirieddate FROM coupon WHERE code = @code And id = @id";
            return _dbConnection.QuerySingleOrDefault<Coupon>(sql, new { Code, id });
        }

        public void CreateCoupon(Coupon coupon)
        {
            _dbConnection.Insert(_tableName, coupon);
        }

        public void RegiserCoupon(MemberCoupon memberCoupon)
        {
            var sql = "INSERT INTO membercoupon (id,coupon_id,member_id,expired_at,is_used,issued_at) VALUES (@id,@couponId,@memberId,@ExpiredAt,@IsUsed,@IssuedAt)";
            _logger.LogInformation("{sql}", sql);
            _logger.LogInformation("{couponId}", memberCoupon);
            _dbConnection.Execute(sql, new { memberCoupon.Id, memberCoupon.CouponId, memberCoupon.MemberId, memberCoupon.ExpiredAt, memberCoupon.IsUsed, memberCoupon.IssuedAt });
        }
        public void UpdateCoupon(Coupon coupon)
        {
            _dbConnection.Update(_tableName, coupon);
        }

        public void DeleteCoupon(Guid id)
        {
            _dbConnection.Delete(_tableName, id);
        }


    }
}