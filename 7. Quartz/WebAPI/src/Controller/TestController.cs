using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.src.Controller;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase {
    [HttpGet]
    public IActionResult GetOk() {
        return Ok("Indo");
    }
}
