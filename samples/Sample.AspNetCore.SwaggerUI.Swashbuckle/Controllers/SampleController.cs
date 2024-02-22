using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Controllers;

[ApiController]
public class SampleController : ControllerBase
{
    [HttpHead("head")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult HeadSample([Required] int sample)
    {
        return Ok(sample);
    }

    [HttpPatch("patch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public IActionResult PatchSample(Sample model)
    {
        return Ok(model);
    }

    [HttpOptions("options")]
    [Authorize]
    public IActionResult OptionsSample()
    {
        return Ok();
    }

    [HttpPost("form")]
    [Authorize]
    public IActionResult PostSample([FromForm] Sample model, IFormFile form)
    {
        return Ok();
    }

    [HttpGet("get")]
    [Obsolete("test")]
    public IActionResult GetDisabledSample([Required] int sample)
    {
        return Ok(sample);
    }
}