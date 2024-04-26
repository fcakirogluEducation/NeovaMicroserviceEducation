using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeovaMicroserviceOrder.API.Models;

namespace NeovaMicroserviceOrder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(context.Products.ToList());
        }
    }
}