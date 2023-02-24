using System.Globalization;
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

        _logger.LogInformation("Escrevendo log de info");
        _logger.LogWarning("Escrevendo log de warning");
        _logger.LogError("Escrevendo log de erro");

        var frase = $"Nessa requisição get, veio sem parametros.";
        return Ok(frase);
    }

    [HttpGet("{param}")] //caminho Route + "/{id}"
    public IActionResult Get(int param)
    {
        _logger.LogWarning("solicitando frase por parametro");

        if (param == 0)
        {
            throw new DivideByZeroException("Param não pode ser 0");
        }

        var frase = $"Foi informado o ID {param} nessa requisição GET.";

        return Ok(frase);
    }


}
