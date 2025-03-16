namespace 水水水果API.Models
{
    public record MemberCoupon
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("member_id")]
        public Guid? MemberId { get; set; }
        [Column("coupon_id")]
        public Guid? CouponId { get; set; }
        [Column("expired_at")]
        public DateTime? ExpiredAt { get; set; }
        [Column("is_used")]
        public bool? IsUsed { get; set; }
        [Column("issued_at")]
        public DateTime? IssuedAt { get; set; }

        public Member Member { get; set; }
        public Coupon Coupon { get; set; }
    }
}
