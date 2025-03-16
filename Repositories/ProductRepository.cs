namespace 水水水果API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _tableName = "product";
        public ProductRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<Product> GetProducts()
        {

            return _dbConnection.GetAll<Product>(_tableName);
        }

        public Product GetProductById(Guid id)
        {
            return _dbConnection.GetById<Product>(_tableName, id);
        }

        public void CreateProduct(Product product)
        {
            _dbConnection.Insert(_tableName, product);
        }

        public void UpdateProduct(Product product)
        {
            _dbConnection.Update(_tableName, product);
        }

        public void DeleteProduct(Guid id)
        {
            _dbConnection.Delete(_tableName, id);
        }
    }
}
