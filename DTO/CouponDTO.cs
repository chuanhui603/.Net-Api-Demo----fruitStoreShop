namespace 水水水果API.DTO
{
    public class CouponDTO
    {
        public Guid? Id { get; set; } // Changed to Guid
        public string Code { get; set; }
        public string Name { get; set; } // Add this line
        public decimal? DiscountAmount { get; set; }
        public DateTime? ExpiriedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}