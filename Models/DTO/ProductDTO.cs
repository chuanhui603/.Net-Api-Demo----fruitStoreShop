namespace 水水水果API.Models.DTO
{
    public record ProductDTO
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public int StoreId { get; set; }

        public DateTime Description { get; set; }

        public string Code { get; set; }

        public decimal Price { get; set; }

        public string CreateDate { get; set; }

        public string LastUpdateDate { get; set; }

        public bool IsActive { get; set; }

    }
}
