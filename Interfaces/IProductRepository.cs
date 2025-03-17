namespace 水水水果API.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsByPage(int page, int pageSize);
        Product GetProductById(Guid id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Guid id);
    }
}
