namespace 水水水果API.Models
{
    public class OrderDetail
    {
        [Column("id")]
        public Guid Id { get; set; } // Changed to Guid  
        [Column("orderid")]
        public Guid? OrderId { get; set; } // Changed to Guid
        [Column("productid")]
        public Guid? ProductId { get; set; } // Changed to Guid
        [Column("price")]
        public decimal? Price { get; set; }
        [Column("quantity")]
        public int? Quantity { get; set; }
        [Column("subtotal")]
        public decimal? Subtotal { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        public Product Product { get; set; }
    }
}
