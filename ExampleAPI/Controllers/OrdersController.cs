using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Controllers;

[Route("api/[controller]")]
public class OrdersController : Controller
{
    private IOrderRepository _orderRepository;
    private IStockRepository _stockRepository;

    public OrdersController(IOrderRepository orderRepository, IStockRepository stockRepository)
    {
        _orderRepository = orderRepository;
        _stockRepository = stockRepository;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        return Ok(_orderRepository.GetAll());
    }

    [HttpGet("GetAllWithOrderItems")]
    public IActionResult GetAllWithProducts()
    {
        return Ok(_orderRepository
            .GetAll(include: order => order.Include(o => o.OrderItems).Include(o => o.User))
        );
    }

    [HttpGet("GetById/{id}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_orderRepository.Get(order => order.Id == id));
    }

    [HttpPost("Add")]
    public IActionResult Add([FromBody] Order order)
    {
        return Ok(_orderRepository.Add(order));
    }

    [HttpPut("Update")]
    public IActionResult Update([FromBody] Order order)
    {
        return Ok(_orderRepository.Update(order));
    }

    [HttpDelete("DeleteById/{id}")]
    public IActionResult Delete(Guid id)
    {
        var order = _orderRepository.Get(order => order.Id == id);
        if (order == null) return BadRequest("Order not found");
        return Ok(_orderRepository.Delete(order));
    }
}
