using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using DocumentFormat.OpenXml.Packaging;

using Microsoft.AspNetCore.Mvc;
using Teste.src.Services;

namespace YourNamespace.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FilePrintController : ControllerBase {
  private readonly IPrintService _printService;

  public FilePrintController(IPrintService printService) {
    _printService = printService;
  }

  // POST: api/fileprint/print
  [HttpPost("print")]
  public async Task<IActionResult> PrintFile([FromForm] IFormFile file, [FromForm] string printerName) {
    try {
      await _printService.PrintFile(file, printerName);
      return Ok("Impressão realizada com sucesso!");
    } catch (Exception ex) {
      return BadRequest(ex.Message);
    }
  }
}