namespace 水水水果API.Services
{
    internal class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ProductDTO GetProductById(Guid id)
        {
            var product = _productRepository.GetProductById(id);
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                Origin = product.Origin,
                Weight = product.Weight,
                RecommendedStorage = product.RecommendedStorageMethod,
                Description = product.Description,
            };
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return _productRepository.GetProducts().Select(product => new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                Origin = product.Origin,
                Weight = product.Weight,
                RecommendedStorage = product.RecommendedStorageMethod,
                Description = product.Description,
            });
        }
        public void CreateProduct(ProductDTO product)
        {
            _productRepository.CreateProduct(new Product
            {
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                Origin = product.Origin,
                Weight = product.Weight,
                RecommendedStorageMethod = product.RecommendedStorage,
                Description = product.Description,
            });
        }
        public void UpdateProduct(Guid id, ProductDTO product)
        {
            _productRepository.UpdateProduct(new Product
            {
                Id = id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                Origin = product.Origin,
                Weight = product.Weight,
                RecommendedStorageMethod = product.RecommendedStorage,
                Description = product.Description,
            });
        }

        public void DeleteProduct(Guid id)
        {
            _productRepository.DeleteProduct(id);
        }
    }
}