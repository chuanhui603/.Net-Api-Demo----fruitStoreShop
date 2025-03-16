namespace 水水水果API.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetOrders();
        IEnumerable<OrderDTO> GetOrdersByPage(int page, int pageSize);
        OrderDTO GetOrderById(Guid id);
        void CreateOrder(OrderDTO order);
        void UpdateOrder(Guid id, OrderDTO order);
        void DeleteOrder(Guid id);
    }
}
