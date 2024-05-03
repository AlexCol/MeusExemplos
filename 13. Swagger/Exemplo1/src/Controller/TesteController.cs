using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Exemplo1.src.Attributes.SwaggerExclude;

namespace Exemplo1.src.Controller;

[ApiController]
[Route("[controller]")]
[Authorize]
//[SwaggerInclude]
public class TesteController : ControllerBase {
    [HttpGet("")]
    public IActionResult TesteGet() {
        return Ok("ok");
    }

    [HttpPost("")]
    [SwaggerExclude]
    public IActionResult TestePost() {
        return Ok("ok");
    }
}
