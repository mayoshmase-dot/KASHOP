using KASHOP.BLL;
using KASHOP.DAL.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(request, cancellationToken);
            return result ? Ok() : BadRequest("invalid product data");
        }

       
    }
}
