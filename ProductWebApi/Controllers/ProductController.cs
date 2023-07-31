using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebApi.Context;
using ProductWebApi.Models;

namespace ProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _dbContext;

        public ProductController(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<Product>> GetProductById(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{productId:int}")]
        public async Task<ActionResult<Product>> DeleteCustomer(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
