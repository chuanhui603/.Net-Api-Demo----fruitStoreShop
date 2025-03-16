namespace 水水水果API.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductDTO> GetProducts();
        ProductDTO GetProductById(Guid id);
        void CreateProduct(ProductDTO product);
        void UpdateProduct(Guid id, ProductDTO product);
        void DeleteProduct(Guid id);
    }
}
