using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderWebApi.Models;

namespace OrderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderController()
        {
            var dbHost = "localhost";
            var dbName = "db_order";
            var connection = $"mongodb://{dbHost}:27017/{dbName}";

            var mongoUrl = MongoUrl.Create(connection);
            var mongoClient = new MongoClient(mongoUrl);

            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            _orderCollection = database.GetCollection<Order>("order");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _orderCollection.Find(Builders<Order>.Filter.Empty).ToListAsync();
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrderById(string orderId)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.OrderId, orderId);
            return await _orderCollection.Find(filter).SingleOrDefaultAsync();
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            await _orderCollection.InsertOneAsync(order);
            return Ok(order);
        }

        [HttpPut]
        [Authorize(Roles ="Admin,User")]
        public async Task<ActionResult<Order>> UpdateOrder(Order order)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.OrderId, order.OrderId);
            await _orderCollection.ReplaceOneAsync(filter, order);
            return Ok(order);
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder(string orderId)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.OrderId, orderId);
            await _orderCollection.DeleteOneAsync(filter);
            return Ok();
        }
    }
}
