using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorHandler : Controller
{
    private readonly ILogger<ErrorHandler>? _logger;

    public ErrorHandler(ILogger<ErrorHandler> logger)
    {
        _logger = logger;
    }
    [Route("/error")]
    public IActionResult HandleError()
    {
        var error = this.HttpContext.Features?.Get<IExceptionHandlerFeature>()?.Error;

        if (error is DivideByZeroException)
            return BadRequest("Parametro não pode ser zero.");

        return BadRequest("Deu ruim");
    }
}