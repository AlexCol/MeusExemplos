using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.services;
using Microsoft.AspNetCore.Mvc;

namespace InjeçãoDinamica.Properties.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class GenericTestController : ControllerBase {
  private readonly IServiceProvider _serviceProvider;

  public GenericTestController(IServiceProvider serviceProvider) {
    _serviceProvider = serviceProvider;
  }

  [HttpGet("test")]
  public ActionResult<string> Test() {
    var strService = _serviceProvider.GetRequiredService<IGenericService<string>>();
    var intService = _serviceProvider.GetRequiredService<IGenericService<int>>();

    var result = new {
      StringResult = strService.Show("Hello, World!"),
      IntResult = intService.Show(42)
    };

    return Ok(result);
  }
}
