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

    [HttpGet("{param}")] //caminho Route + "/{id}"
    public IActionResult Get(string param)
    {
        var frase = $"Foi informado o ID {param} nessa requisição GET.";

        return Ok(frase);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var frase = $"Nessa requisição get, veio sem parametros.";
        return Ok(frase);
    }

    [HttpPost]
    public IActionResult Post([FromBody] FraseRequest fraseRequest)
    {
        try
        {
            var frase = $"Nessa requisição post, veio no corpo uma FraseRequest, mapeada a apartir de um JSON vindo no body. Frase: {fraseRequest.frase} e OutraCoisa: {fraseRequest.outraCoisa}";

            return Created("url de onde acessar o novo item criado", frase);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}
