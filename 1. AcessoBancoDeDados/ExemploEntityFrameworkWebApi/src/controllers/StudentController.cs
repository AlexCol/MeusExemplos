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
public class CourseController : ControllerBase {
  private readonly IGenericService<Course> _service;

  public CourseController(IGenericService<Course> service) {
    _service = service;
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Course newCourse) {
    await _service.Create(newCourse);
    return Created();
  }
}
