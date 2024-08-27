using Microsoft.AspNetCore.Mvc;

namespace SimpleApi.src.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase {
  [HttpGet()]
  public IActionResult MyTest() {
    var rand = new Random();
    int num = rand.Next(1, 12);
    if (num % 4 == 0) { //4 8 12
      return StatusCode(500, $"Ocorreu algum erro com o código {num}.");
    } else if (num % 3 == 0) { //3 6 9 (12 já foi no de cima)
      return NotFound($"Codigo {num} não existe!");
    } else if (num % 2 == 0) { //2 10
      return BadRequest($"Erro ao solicitar {num}!");
    } else { //1 5 7 11
      return Ok(new { NumeroGerado = num });
    }
  }
}

