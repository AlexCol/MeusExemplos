using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.services;
using ExemploEntityFrameworkWebApi.src.services.Generic;
using ExemploEntityFrameworkWebApi.src.util;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase {
  private readonly IGenericService<Student> _service;

  public StudentController(IGenericService<Student> service) {
    _service = service;
  }

  [HttpGet("search")]
  public async Task<IActionResult> Search([FromBody] JsonElement searchParams) {
    var criteria = CriteriaConverter.ConvertJsonElementToCriteria(searchParams);
    return Ok(await _service.SeachByCriteria(criteria));
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Student newStudent) {
    await _service.Create(newStudent);
    return Created();
  }
}
