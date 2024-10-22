using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PrinterController : ControllerBase
{
  // GET: api/printer/list
  [HttpGet("list")]
  public IActionResult ListPrinters()
  {
    List<string> printers = new List<string>();

    foreach (string printer in PrinterSettings.InstalledPrinters)
    {
      printers.Add(printer);
    }

    return Ok(printers);
  }
}
