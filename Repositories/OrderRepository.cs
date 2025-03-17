using Order = 水水水果API.Models.Order;
namespace 水水水果API.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _tableName = "order";

        public OrderRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IEnumerable<Order> GetOrdersByPage(int page, int pageSize)
        {
           return _dbConnection.GetByPage<Order>(_tableName, page, pageSize);
        }
        public IEnumerable<Order> GetOrders()
        {
            return _dbConnection.GetAll<Order>(_tableName);
        }

        public Order GetOrderById(Guid id)
        {
            return _dbConnection.GetById<Order>(_tableName, id);
        }

        public void CreateOrder(Order order)
        {
            _dbConnection.Insert(_tableName, order); ;
        }

        public void UpdateOrder(Order order)
        {
            _dbConnection.Update(_tableName, order);
        }

        public void DeleteOrder(Guid id)
        {
            _dbConnection.Delete(_tableName, id);
        }


    }
}