namespace 水水水果API.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; } // Changed to Guid
        public Guid MemberId { get; set; } // Changed to Guid
        public Guid RecipientId { get; set; } // Changed to Guid
        public Guid MemberCouponId { get; set; } // Changed to Guid
        public string OrderNumber { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public string DeliveryStatus { get; set; }
        public int? ProductAmount { get; set; }
        public int? DiscountAmount { get; set; }
        public int? ShippingFee { get; set; }
        public int? TotalAmount { get; set; }
        public string Notes { get; set; }
    }
}