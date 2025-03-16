using Order = 水水水果API.Models.Order;
namespace 水水水果API.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        IEnumerable<Order> GetOrdersByPage(int page, int pageSize);
        Order GetOrderById(Guid id);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Guid id);
    }
}