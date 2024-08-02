using System.Text.Json;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.services.Generic;
using ExemploEntityFrameworkWebApi.src.util;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase {
  private readonly IGenericService<Course> _service;

  public CourseController(IGenericService<Course> service) {
    _service = service;
  }

  [HttpGet("search")]
  public async Task<IActionResult> Search([FromBody] JsonElement searchParams) {
    var criteria = CriteriaConverter.ConvertJsonElementToCriteria(searchParams);
    return Ok(await _service.SeachByCriteria(criteria));
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Course newCourse) {
    await _service.Create(newCourse);
    return Created();
  }
}
