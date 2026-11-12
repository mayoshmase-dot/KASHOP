using KASHOP.BLL;
using KASHOP.DAL.Data;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return result ? Ok() :BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id , cancellationToken);
            return result is not null ? Ok(result) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.RemoveAsync(id, cancellationToken);
            return result ? NoContent() : BadRequest();
        }

        /* [HttpPatch("")]
         public IActionResult Update(Category category)
         {
             _service.Update(category);
             return Ok();
         }*/
    }
}

