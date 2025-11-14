using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDTO> GetProducts();
        IEnumerable<ProductDTO> GetProductsByPage(int page, int pageSize);
        ProductDTO GetProductById(int id);
        void CreateProduct(ProductDTO product);
        void UpdateProduct(int id, ProductDTO product);
        void DeleteProduct(int id);
    }
}
