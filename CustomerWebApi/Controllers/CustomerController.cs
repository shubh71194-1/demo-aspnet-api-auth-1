using CustomerWebApi.Context;
using CustomerWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbContext _dbContext;

        public CustomerController(CustomerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int customerId)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);
            return Ok(customer);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpPut]
        [Authorize(Roles ="Admin,User")]
        public async Task<ActionResult<Customer>> UpdateCustomer(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpDelete("{customerId:int}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int customerId)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);
            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
