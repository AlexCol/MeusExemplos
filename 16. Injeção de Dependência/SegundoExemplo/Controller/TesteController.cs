using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SegundoExemplo.src.Interfaces;
using SegundoExemplo.src.Model;

namespace SegundoExemplo.Controller;

[ApiController]
[Route("[controller]")]
public class TesteController : ControllerBase {

    IService _scoped;
    SingletonService _singleton;
    TransientService _transient;
    public TesteController(
        IService scoped,
        SingletonService singleton,
        TransientService transient
    ) {
        _scoped = scoped;
        _singleton = singleton;
        _transient = transient;
    }

    [HttpGet("1")]
    public IActionResult PrimeiroTeste() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(_singleton.PrintId());
        builder.AppendLine(_transient.PrintId());
        builder.AppendLine(_scoped.PrintId());
        return Ok(builder.ToString());
    }

    [HttpGet("2")]
    public IActionResult SegundoTeste() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(_singleton.PrintId());
        builder.AppendLine(_transient.PrintId());
        builder.AppendLine(_scoped.PrintId());
        return Ok(builder.ToString());
    }
}
