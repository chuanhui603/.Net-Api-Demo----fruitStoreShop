namespace 水水水果API.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public string Origin { get; set; }
        public string RecommendedStorage { get; set; }
        public string SupplyMethod { get; set; }
        public decimal? Price { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}
