using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Teste.src.Model;
using Teste.src.Services;

namespace Teste.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{

    private readonly IPrintService _printService;

    public ArticlesController(IPrintService printService)
    {
        _printService = printService;
    }

    [HttpPost]
    public IActionResult Get([FromBody] IEnumerable<Article> articles)
    {
        _printService.PrintGeneratedPdf(articles, "CutePDF Writer");
        return Ok(articles);
    }
}
