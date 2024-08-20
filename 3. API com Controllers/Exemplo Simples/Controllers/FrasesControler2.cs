using Microsoft.AspNetCore.Mvc;


namespace Controllers;

[ApiController]
[Route("api/frases")]
public class FrasesControler2 : ControllerBase
{
    private readonly ILogger<FrasesControler2> _logger;

    public FrasesControler2(ILogger<FrasesControler2> logger)
    {
        _logger = logger;
    }

    [HttpPut]
    public IActionResult Put([FromBody] FraseRequest fraseRequest, [FromQuery] string par1, [FromQuery] string par2)
    {
        try
        {
            var frase = $"Nessa requisição put, veio no corpo uma FraseRequest, mapeada a apartir de um JSON vindo no body. Frase: {fraseRequest.frase} e OutraCoisa: {fraseRequest.outraCoisa}";
            frase += $" além disso, vieram como parametros no query url {par1} e {par2}";

            return Created("url de onde acessar o novo item criado", frase);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}
