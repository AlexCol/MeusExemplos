using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase {
  [HttpGet]
  public ActionResult Teste() {
    return Ok("Funcionando!");
  }
}
