using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Users")]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Gets all users
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new[] { new { Id = 1, Name = "John" }, new { Id = 2, Name = "Jane" } });
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(new { Id = id, Name = "John Doe" });
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] object user)
    {
        return CreatedAtAction(nameof(GetById), new { id = 1 }, user);
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] object user)
    {
        return NoContent();
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return NoContent();
    }
}