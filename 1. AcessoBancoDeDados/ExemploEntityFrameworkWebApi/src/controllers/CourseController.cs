using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.services;
using ExemploEntityFrameworkWebApi.src.services.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase {
  private readonly IGenericService<Student> _service;

  public StudentController(IGenericService<Student> service) {
    _service = service;
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Student newStudent) {
    await _service.Create(newStudent);
    return Created();
  }
}
