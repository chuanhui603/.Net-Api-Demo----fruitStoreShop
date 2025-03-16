namespace 水水水果API.Models
{
    public record Coupon
    {
        [Column("id")]
        public Guid Id { get; set; } 
        [Column("code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; } 
        [Column("discount_amount")]
        public decimal? DiscountAmount { get; set; }
        [Column("expirieddate")]
        public DateTime? ExpiriedDate { get; set; }
        [Column("isactive")]
        public bool? IsActive { get; set; }
    }
}