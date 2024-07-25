using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.error;
using ExemploEntityFrameworkWebApi.src.services;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase {
  private readonly IPersonService _service;

  public PersonController(IPersonService service) {
    _service = service;
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> FindById(int id) {
    try {
      return Ok(await _service.FindById(id));
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpGet("name/{name}")]
  public async Task<IActionResult> FindByName(string name) {
    try {
      return Ok(await _service.FindByName(name));
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpGet("list")]
  public async Task<IActionResult> FindAll() {
    try {
      var list = await _service.FindAll();
      if (list.Count == 0) return NoContent();
      return Ok(list);
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Person newPerson) {
    try {
      var person = await _service.Create(newPerson);
      return Created($"person/{person.Id}", null);
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }
}
