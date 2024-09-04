using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.services.httpServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class TestRequestController : ControllerBase {
  private readonly ITestApiRequests _request;

  public TestRequestController(ITestApiRequests request) {
    _request = request;
  }

  [HttpGet]
  public async Task<IActionResult> Get() {
    Console.WriteLine("Entrou no controller.");
    var retorno = await _request.SendAsync("test");
    return Ok(retorno);
  }
}
