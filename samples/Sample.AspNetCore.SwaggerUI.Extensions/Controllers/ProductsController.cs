using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Products")]
public class ProductsController : ControllerBase
{
    /// <summary>
    /// Gets all products
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new[] { new { Id = 1, Name = "Product A" }, new { Id = 2, Name = "Product B" } });
    }

    /// <summary>
    /// Gets a product by ID
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(new { Id = id, Name = "Product", Price = 99.99 });
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] object product)
    {
        return CreatedAtAction(nameof(GetById), new { id = 1 }, product);
    }
}