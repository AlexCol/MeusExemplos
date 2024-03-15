using Microsoft.AspNetCore.Mvc;
using SimpleApi.src.Model;

namespace SimpleApi.src.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase {
  [HttpGet()]
  public IActionResult MyTest() {
    StaticValue.Value += 1;
    if (StaticValue.Value % 3 == 0) {
      return BadRequest("Bau bau");
    } else {
      return Ok(new { StaticValue.Value });
    }
  }
}
