using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using 水水水果API.Interfaces;
using 水水水果API.Models.DTO;
using 水水水果API.Models.RMS;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void GetProducts_ReturnsDtos()
        {
            var repo = new Mock<IProductRepository>();
            var logger = NullLogger<ProductService>.Instance;
            repo.Setup(r => r.GetProducts()).Returns(new List<Product> { new Product(), new Product() });

            var service = new ProductService(repo.Object, logger);
            var result = service.GetProducts();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetProductsByPage_ReturnsDtos()
        {
            var repo = new Mock<IProductRepository>();
            var logger = NullLogger<ProductService>.Instance;
            repo.Setup(r => r.GetProductsByPage(1, 5)).Returns(new List<Product> { new Product() });

            var service = new ProductService(repo.Object, logger);
            var result = service.GetProductsByPage(1, 5);

            Assert.Single(result);
        }

        [Fact]
        public void GetProductById_ReturnsDto()
        {
            var repo = new Mock<IProductRepository>();
            var logger = NullLogger<ProductService>.Instance;
            repo.Setup(r => r.GetProductById(3)).Returns(new Product { Id = 3 });

            var service = new ProductService(repo.Object, logger);
            var result = service.GetProductById(3);

            Assert.NotNull(result);
        }

        [Fact]
        public void CreateProduct_LogsWithoutRepositoryCall()
        {
            var repo = new Mock<IProductRepository>();
            var logger = NullLogger<ProductService>.Instance;

            var service = new ProductService(repo.Object, logger);
            service.CreateProduct(new ProductDTO { Id = 1, Code = "ABC" });

            repo.Verify(r => r.CreateProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void UpdateProduct_InvokesRepository()
        {
            var repo = new Mock<IProductRepository>();
            var logger = NullLogger<ProductService>.Instance;
            var service = new ProductService(repo.Object, logger);

            service.UpdateProduct(2, new ProductDTO { Id = 2 });

            repo.Verify(r => r.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void DeleteProduct_InvokesRepository()
        {
            var repo = new Mock<IProductRepository>();
            var logger = NullLogger<ProductService>.Instance;
            var service = new ProductService(repo.Object, logger);

            service.DeleteProduct(7);

            repo.Verify(r => r.DeleteProduct(7), Times.Once);
        }
    }
}
