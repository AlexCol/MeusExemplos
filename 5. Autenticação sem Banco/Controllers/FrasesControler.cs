using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/frases")]
public class FrasesControler : ControllerBase
{
    private readonly ILogger<FrasesControler> _logger;

    public FrasesControler(ILogger<FrasesControler> logger)
    {
        _logger = logger;
    }

    [HttpGet("{param}")] //caminho Route + "/{id}"
    public IActionResult Get(string param)
    {
        _logger.LogWarning("solicitando frase por parametro");

        var frase = $"Foi informado o ID {param} nessa requisição GET.";

        return Ok(frase);
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogWarning("solicitando frase sem parametro");

        _logger.LogError(this.HttpContext.User.Claims.Count().ToString());
        foreach (var item in this.HttpContext.User.Claims)
        {
            if (item.Type == ClaimTypes.NameIdentifier)
                _logger.LogWarning($"NameIdentifier = {item.Value}");
            else if (item.Type == ClaimTypes.Email)
                _logger.LogWarning($"Email = {item.Value}");
            else if (item.Type == "MinhaVariavel")
                _logger.LogWarning($"MinhaVariavel = {item.Value}");
            else
                _logger.LogError("não é identificador do token.");
        }

        var frase = $"Nessa requisição get, veio sem parametros.";
        return Ok(frase);
    }
}
