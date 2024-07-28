using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.error;
using ExemploEntityFrameworkWebApi.src.services;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("[controller]")]
/*
  REMOVIDOS TODOS OS TRY CATCHES, POIS FOI CRIADO UM MIDDLEWARE PARA CAPTURAR EXCESSÕES DISPARADAS E NÃO TRATADAS
*/
public class PersonController : ControllerBase {
  private readonly IPersonService _service;

  public PersonController(IPersonService service) {
    _service = service;
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> FindById(int id) {
    return Ok(await _service.FindById(id));
  }

  [HttpGet("name/{name}")]
  public async Task<IActionResult> FindByName(string name) {
    return Ok(await _service.FindByName(name));
  }

  [HttpGet("birth/{birth}")]
  public async Task<IActionResult> FindByBirth(DateTime birth) {
    return Ok(await _service.FindByBirth(birth));
  }

  [HttpGet("gender/{genderId}")]
  public async Task<IActionResult> FindByGender(int genderId) {
    return Ok(await _service.FindByGender(genderId));
  }

  [HttpGet("list")]
  public async Task<IActionResult> FindAll() {
    var list = await _service.FindAll();
    if (list.Count == 0) return NoContent();
    return Ok(list);
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Person newPerson) {
    var person = await _service.Create(newPerson);
    return Created($"person/{person.Id}", null);
  }

  [HttpPut("update/{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] Person person) {
    person.Id = id;
    await _service.Update(person);
    return NoContent();
  }

  [HttpDelete("delete/{id}")]
  public async Task<IActionResult> Delete(int id) {
    await _service.DeleteById(id);
    return NoContent();
  }
}
