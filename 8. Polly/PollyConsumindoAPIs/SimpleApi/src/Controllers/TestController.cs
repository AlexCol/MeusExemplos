using Microsoft.AspNetCore.Mvc;
using SimpleApi.src.Model;

namespace SimpleApi.src.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase {
  [HttpGet()]
  public async Task<IActionResult> MyTest() {
    StaticValue.Value += 1;
    await Task.Delay(1);
    if (StaticValue.Value % 4 != 0) {
      return BadRequest("Ocorreu algum erro.");
    } else {
      return Ok(new { StaticValue.Value });
    }
  }
}
