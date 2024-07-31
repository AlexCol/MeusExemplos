using System;
using System.Collections.Generic;
using System.Text.Json;
using ExemploEntityFrameworkWebApi.src.util;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers {
  [ApiController]
  [Route("[controller]")]
  public class TestController : ControllerBase {
    [HttpGet]
    public ActionResult Teste([FromBody] JsonElement parameters) {
      var tuples = CriteriaConverter.ConvertJsonElementToCriteria(parameters);
      return Ok(tuples);
    }
  }
}
