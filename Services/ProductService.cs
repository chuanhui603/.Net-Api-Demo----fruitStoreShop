using 水水水果API.Models.DTO;

namespace 水水水果API.Services
{
    internal class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private ILogger<ProductService> _logger;
        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public ProductDTO GetProductById(int id)
        {
            var product = _productRepository.GetProductById(id);
            return new ProductDTO
            {
            
            };
        }

        public IEnumerable<ProductDTO> GetProductsByPage(int page, int pageSize)
        {
            return _productRepository.GetProductsByPage(page, pageSize).Select(product => new ProductDTO
            {
              
            });
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return _productRepository.GetProducts().Select(product => new ProductDTO
            {
              
            });
        }
        public void CreateProduct(ProductDTO product)
        {
            _logger.LogInformation("product: {product}", product);
            
            // _productRepository.CreateProduct(new Product
            // {
            //     Id = Guid.NewGuid(),
            //     Name = product.Name,
            //     Price = product.Price,
            //     Category = product.Category,
            //     Origin = product.Origin,
            //     Weight = product.Weight,
            //     RecommendedStorageMethod = product.RecommendedStorageMethod,
            //     Description = product.Description,
            //     UpdatedAt = DateTime.UtcNow,
            //     CreatedAt = DateTime.UtcNow,
            // });
        }
        public void UpdateProduct(int id, ProductDTO product)
        {
            _productRepository.UpdateProduct(new Product
            {
           
            });
        }

        public void DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
        }


    }
}