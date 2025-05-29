using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.services;
using Microsoft.AspNetCore.Mvc;

namespace InjeçãoDinamica.Properties.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class AppController : ControllerBase {
  private readonly IAppService _service;

  public AppController(IAppService service) {
    _service = service;
  }

  [HttpGet("hello")]
  public IActionResult SayHello() {
    return Ok(new { message = _service.SayHello() });
  }
}
