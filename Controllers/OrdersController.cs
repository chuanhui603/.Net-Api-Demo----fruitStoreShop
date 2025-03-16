using Microsoft.AspNetCore.Authorization;

namespace 水水水果.Controllers
{
    [Route("api/[controller]")]
     [ServiceFilter(typeof(LogoutActionFilter))]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public IActionResult Get()
        {
            var orders = _orderService.GetOrders();
            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("page={page}/top={pageSize}")]
        public IActionResult GetById(Guid id)
        {
            var order = _orderService.GetOrdersByPage(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }


        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public IActionResult GetByPage(int page,int pageSize)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public IActionResult Post([FromBody] OrderDTO order)
        {
            _orderService.CreateOrder(order);
            return Created();
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] OrderDTO order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }
            _orderService.UpdateOrder(id, order);
            return NoContent();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _orderService.DeleteOrder(id);
            return NoContent();
        }
    }
}
