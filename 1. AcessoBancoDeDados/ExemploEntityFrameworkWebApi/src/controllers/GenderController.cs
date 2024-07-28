using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.error;
using ExemploEntityFrameworkWebApi.src.services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("[controller]")]
public class GenderController : ControllerBase {
  private readonly IGenderService _service;

  public GenderController(IGenderService service) {
    _service = service;
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> FindById(int id) {
    try {
      return Ok(await _service.FindById(id)); //se nulo, retorno NoContent
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e));
    }
  }

  [HttpGet("description/{description}")]
  public async Task<IActionResult> FindByDescription(string description) {
    var list = await _service.FindByDescriptionCaseInsentive(description);
    if (list.Count == 0) return NoContent();
    return Ok(list);
  }

  [HttpGet("list")]
  public async Task<IActionResult> ListAll() {
    var list = await _service.FindAll();
    if (list.Count == 0) return NoContent();
    return Ok(list);
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Gender newGender) {
    var gender = await _service.Create(newGender);
    return Created($"/{gender.Id}", null);
  }

  [HttpPut("update/{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] Gender gender) {
    gender.Id = id;
    await _service.Update(gender);
    return NoContent();
  }

  [HttpDelete("delete/{id}")]
  public async Task<IActionResult> Delete(int id) {
    await _service.DeleteById(id);
    return NoContent();
  }
}
