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
        GenericRepository<Brand> repo = new GenericRepository<Brand>();

        [HttpGet("")]
        public IActionResult Index()
        {
            var brands = repo.Get();
            return Ok(brands);
        }
        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var brand = context.Brands.Find(id);
        //    return Ok(brand);
        //}
        [HttpPost("")]
        public IActionResult Create(Brand brand)
        {
            repo.Create(brand);
            return Ok();
        }

        /* [HttpPatch("")]
         public IActionResult Update(Brand brand)
         {
            repo.Update(brand);
             return Ok();
         }*/
    }
}
