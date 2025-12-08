
namespace 水水水果API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TWRMS_TESTContext _rmsConnection;
        public ProductRepository(TWRMS_TESTContext rmsConnection)
        {
            _rmsConnection = rmsConnection;
        }

        public IEnumerable<Product> GetProducts()
        {

            return [.. _rmsConnection.Products];
        }
        public IEnumerable<Product> GetProductsByPage(int page, int pageSize)
        {
           return [.. _rmsConnection.Products.Skip(page).Take(pageSize)];
        }
        public Product GetProductById(int id)
        {
            return _rmsConnection.Products.Where(x => x.Id == id).First();
        }

        public void CreateProduct(Product product)
        {
            _rmsConnection.Products.Add(product);
            _rmsConnection.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _rmsConnection.Products.Update(product);
            _rmsConnection.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
           var product =  _rmsConnection.Products.Where(x=>x.Id == id).First();
            _rmsConnection.Products.Remove(product);
        }
    }
}
