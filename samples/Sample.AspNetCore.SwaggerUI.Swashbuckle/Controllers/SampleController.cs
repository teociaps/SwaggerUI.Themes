#pragma warning disable S1133 // Deprecated code should be removed - Done for testing purposes
#pragma warning disable CS0618 // Type or member is obsolete - Done for testing purposes

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Controllers;

[ApiController]
[Route("sample")]
public class SampleController : ControllerBase
{
    [HttpHead("head")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult HeadSample([Required] int sample)
    {
        return Ok(sample);
    }

    [HttpPatch("patch")]
    [ProducesResponseType(typeof(Models.Sample), StatusCodes.Status200OK)]
    [Produces("application/json")]
    public IActionResult PatchSample(Models.Sample model)
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
    public IActionResult PostSample([FromForm] Models.Sample model, IFormFile form)
    {
        return Ok();
    }

    [HttpGet("get")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [Obsolete("test")]
    public IActionResult GetDisabledSample([Required] int sample)
    {
        return Ok(sample);
    }
}