using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpGet("description/{description}")]
  public async Task<IActionResult> FindByDescription(string description) {
    try {
      var list = await _service.FindByDescriptionCaseInsentive(description);
      if (list.Count == 0) return NoContent();
      return Ok(list);
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpGet("list")]
  public async Task<IActionResult> ListAll() {
    try {
      var list = await _service.FindAll();
      if (list.Count == 0) return NoContent();
      return Ok(list);
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] Gender newGender) {
    try {
      var gender = await _service.Create(newGender);
      return Created($"/{gender.Id}", null);
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpPut("update/{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] Gender gender) {
    try {
      gender.Id = id;
      await _service.Update(gender);
      Log.Error("here");
      return NoContent();
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }

  [HttpDelete("delete/{id}")]
  public async Task<IActionResult> Delete(int id) {
    try {
      await _service.DeleteById(id);
      return NoContent();
    } catch (Exception e) {
      return BadRequest(new ErrorModel(e.Message));
    }
  }
}
