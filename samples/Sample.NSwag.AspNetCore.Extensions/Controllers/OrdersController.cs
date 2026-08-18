using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Orders")]
public class OrdersController : ControllerBase
{
    /// <summary>
    /// Gets all orders
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new[] { new { Id = 1, Total = 150.00 }, new { Id = 2, Total = 200.00 } });
    }

    /// <summary>
    /// Gets an order by ID
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(new { Id = id, Total = 150.00, Status = "Pending" });
    }
}