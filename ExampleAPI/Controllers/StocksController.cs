using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : Controller
    {
        private IStockRepository _stockRepository;
        private IOrderRepository _orderRepository;

        public StocksController(IStockRepository stockRepository, IOrderRepository orderRepository)
        {
            _stockRepository = stockRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_stockRepository.GetAll());
        }

        [HttpGet("GetAllWithProduct")]
        public IActionResult GetAllWithProducts()
        {
            return Ok(_stockRepository
                .GetAll(include: stock => stock.Include(s => s.Product))
            );
        }

        [HttpGet("GetById/{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_stockRepository.Get(stock => stock.Id == id));
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] Stock stock)
        {
            return Ok(_stockRepository.Add(stock));
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] Stock stock)
        {
            var oldStock = _stockRepository.Get(s => s.Id == stock.Id);

            if (oldStock != null)
            {
                oldStock.Amount += stock.Amount;
                return Ok(_stockRepository.Update(oldStock));
            }

            return BadRequest("Stock data is invalid.");
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult Delete(Guid id)
        {
            var stock = _stockRepository.Get(stock => stock.Id == id);
            if (stock == null) return BadRequest("Stock not found");
            return Ok(_stockRepository.Delete(stock));
        }
    }
}
