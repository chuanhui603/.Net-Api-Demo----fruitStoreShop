namespace 水水水果API.Models
{
    public class Order
    {
        [Column("id")]
        public Guid Id { get; set; } // Changed to Guid

        [Column("member_id")]
        public Guid MemberId { get; set; } // Changed to Guid

        [Column("recipient_id")]
        public Guid? RecipientId { get; set; } // Changed to Guid

        [Column("membercoupon_id")]
        public Guid? MemberCouponId { get; set; } // Changed to Guid

        [Column("order_number")]
        public string OrderNumber { get; set; }

        [Column("order_status")]
        public string OrderStatus { get; set; }

        [Column("payment_status")]
        public string PaymentStatus { get; set; }

        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("payment_time")]
        public DateTime? PaymentTime { get; set; }

        [Column("delivery_method")]
        public string DeliveryMethod { get; set; }

        [Column("estimated_delivery_time")]
        public DateTime? EstimatedDeliveryTime { get; set; }

        [Column("actual_delivery_time")]
        public DateTime? ActualDeliveryTime { get; set; }

        [Column("delivery_status")]
        public string DeliveryStatus { get; set; }

        [Column("product_amount")]
        public decimal? ProductAmount { get; set; }

        [Column("discount_amount")]
        public decimal? DiscountAmount { get; set; }

        [Column("shipping_fee")]
        public decimal? ShippingFee { get; set; }

        [Column("total_amount")]
        public decimal? TotalAmount { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        public Member Member { get; set; }
        public Recipient Recipient { get; set; }
        public MemberCoupon MemberCoupon { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}