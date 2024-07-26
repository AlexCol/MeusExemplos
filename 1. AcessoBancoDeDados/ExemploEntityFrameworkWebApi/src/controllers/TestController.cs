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
    var descrcao = "MinhaDescricaoPequena";
    descrcao = descrcao.Substring(0, Math.Min(descrcao.Length - 1, 100));

    var longa = "MinhaDescricaoPequenaMinhaDescricaoPequenaMinhaDescricaoPequenaMinhaDescricaoPequenaMinhaDescricaoPequenaMinhaDescricaoPequena";
    longa = longa.Length <= 100 ? longa : longa.Substring(0, 100);
    //longa = longa.Substring(0, Math.Min(longa.Length - 1, 100));
    return Ok(new {
      descrcao,
      longa
    });
  }
}