using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.services;
using Microsoft.AspNetCore.Mvc;

namespace InjeçãoDinamica.Properties.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class ScopeTestController : ControllerBase {
  private readonly IScopedService _scopedService;
  private readonly ITransientService _transientService;
  private readonly ISingletonService _singletonService;
  private readonly IServiceProvider _serviceProvider;

  public ScopeTestController(IScopedService scopedService, ITransientService transientService, ISingletonService singletonService, IServiceProvider serviceProvider) {
    _scopedService = scopedService;
    _transientService = transientService;
    _singletonService = singletonService;
    _serviceProvider = serviceProvider;
  }

  [HttpGet("scoped")]
  public IActionResult TestScoped() {
    var guid1 = _scopedService.GetGuid();
    var _scopedService2 = _serviceProvider.GetRequiredService<IScopedService>();
    var guid2 = _scopedService2.GetGuid();

    var guidSingleton = _singletonService.GetGuid();

    var result = new {
      mensagem = "Devem ser iguais",
      guid1,
      guid2,
      guidSingleton
    };

    return Ok(result);
  }

  [HttpGet("transient")]
  public IActionResult TestTransient() {
    var guid1 = _transientService.GetGuid();
    var _transientService2 = _serviceProvider.GetRequiredService<ITransientService>();
    var guid2 = _transientService2.GetGuid();

    var guidSingleton = _singletonService.GetGuid();

    var result = new {
      mensagem = "Devem ser diferentes",
      guid1,
      guid2,
      guidSingleton
    };

    return Ok(result);
  }
}
