using ApplicationLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // This is just a static list for demonstration purposes.
        private static readonly List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1500 },
            new Product { Id = 2, Name = "Smartphone", Price = 800 },
            new Product { Id = 3, Name = "Tablet", Price = 500 }
        };

        // GET: /product
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(Products);
        }

    }

}
