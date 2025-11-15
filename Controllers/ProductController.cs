using Microsoft.AspNetCore.Authorization;

namespace 水水水果API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogoutActionFilter))]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ProductDTO product)
        {
            _productService.CreateProduct(product);
            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductDTO product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _productService.UpdateProduct(id, product);
            return NoContent();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] int id)
        {
            _logger.LogInformation("Deleting product with ID: {id}", id);
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
