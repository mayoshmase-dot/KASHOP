using KASHOP.DAL.Models;
using KASHOP.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IGenericRepository<Brand> _repository;

        public BrandsController(IGenericRepository<Brand> repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var brands =  _repository.GetAsync();
            return Ok(brands);
        }
        //[HttpGet("{id}")]
        //public IActionResult GetByIdAsync(int id)
        //{
        //    var brand = context.Brands.Find(id);
        //    return Ok(brand);
        //}
        [HttpPost("")]
        public async Task<IActionResult> CreateAsync(Brand brand)
        {
            _repository.CreateAsync(brand);
            return Ok();
        }

        /* [HttpPatch("")]
         public IActionResult Update(Brand brand)
         {
            _repository.Update(brand);
             return Ok();
         }*/
    }
}
