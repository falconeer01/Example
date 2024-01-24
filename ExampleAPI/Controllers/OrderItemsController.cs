using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;
using ExampleAPI.Repositories.Concretes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrderItemsController : Controller
    {
        private IOrderItemRepository _orderItemRepository;
        private IStockRepository _stockRepository;

        public OrderItemsController(IOrderItemRepository orderItemRepository, IStockRepository stockRepository)
        {
            _orderItemRepository = orderItemRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_orderItemRepository.GetAll());
        }

        [HttpGet("GetAllWithProduct")]
        public IActionResult GetAllWithProduct()
        {
            return Ok(_orderItemRepository.GetAll(include: orderItem => orderItem.Include(oi => oi.Product)));
        }

        [HttpGet("GetById/{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_orderItemRepository.Get(orderItem => orderItem.Id == id));
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] OrderItem orderItem)
        {
            var stock = _stockRepository.Get(s => s.ProductId == orderItem.ProductId);

            if (stock.Amount >= orderItem.Quantity)
            {
                stock.Amount -= orderItem.Quantity;
                _stockRepository.Update(stock);
                return Ok(_orderItemRepository.Add(orderItem));
            }

            return BadRequest("We're out of stock");
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] OrderItem orderItem)
        {
            return Ok(_orderItemRepository.Update(orderItem));
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult Delete(Guid id)
        {
            var orderItem = _orderItemRepository.Get(orderItem => orderItem.Id == id);
            if (orderItem == null) return BadRequest("Order item not found");
            return Ok(_orderItemRepository.Delete(orderItem));
        }
    }
}
