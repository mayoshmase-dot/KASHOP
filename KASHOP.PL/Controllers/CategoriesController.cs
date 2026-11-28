using KASHOP.BLL;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Extenstions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var categories = await _service.GetAllAsync(cancellationToken);
            return Ok(categories);
        }
        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryRequest request  , CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToErrorResponse();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id , cancellationToken);
            return result.IsSuccess ? Ok() : result.ToErrorResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _service.RemoveAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToErrorResponse();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryRequest request ,CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(id, request,cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToErrorResponse();

        }

    }
}

