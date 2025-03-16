using Order = 水水水果API.Models.Order;
namespace 水水水果API.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public void CreateOrder(OrderDTO order)
        {
            _orderRepository.CreateOrder(new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                PaymentTime = order.PaymentTime,
                DeliveryMethod = order.DeliveryMethod,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                ActualDeliveryTime = order.ActualDeliveryTime,
                DeliveryStatus = order.DeliveryStatus,
                ProductAmount = order.ProductAmount,
                DiscountAmount = order.DiscountAmount,
                ShippingFee = order.ShippingFee,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
            });
        }
        public void DeleteOrder(Guid id)
        {
            _orderRepository.DeleteOrder(id);
        }
        public OrderDTO GetOrderById(Guid id)
        {
            var order = _orderRepository.GetOrderById(id);
            return new OrderDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                PaymentTime = order.PaymentTime,
                DeliveryMethod = order.DeliveryMethod,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                ActualDeliveryTime = order.ActualDeliveryTime,
                DeliveryStatus = order.DeliveryStatus,
                ProductAmount = order.ProductAmount,
                DiscountAmount = order.DiscountAmount,
                ShippingFee = order.ShippingFee,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
            };
        }
        public IEnumerable<OrderDTO> GetOrders()
        {
            return _orderRepository.GetOrders().Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                PaymentTime = order.PaymentTime,
                DeliveryMethod = order.DeliveryMethod,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                ActualDeliveryTime = order.ActualDeliveryTime,
                DeliveryStatus = order.DeliveryStatus,
                ProductAmount = order.ProductAmount,
                DiscountAmount = order.DiscountAmount,
                ShippingFee = order.ShippingFee,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
            });
        }

        public IEnumerable<OrderDTO> GetOrdersByPage(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Guid id, OrderDTO order)
        {
            _orderRepository.UpdateOrder(new Order
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                PaymentTime = order.PaymentTime,
                DeliveryMethod = order.DeliveryMethod,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                ActualDeliveryTime = order.ActualDeliveryTime,
                DeliveryStatus = order.DeliveryStatus,
                ProductAmount = order.ProductAmount,
                DiscountAmount = order.DiscountAmount,
                ShippingFee = order.ShippingFee,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
            });
        }
    }
}
