using Microsoft.AspNetCore.Mvc;


namespace Controllers;

[ApiController]
[Route("api/frases")]
public class FrasesControler : ControllerBase
{
    private readonly ILogger<FrasesControler> _logger;

    public FrasesControler(ILogger<FrasesControler> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {

        _logger.LogError("Escrevendo log");

        var frase = $"Nessa requisição get, veio sem parametros.";
        return Ok(frase);
    }


}
