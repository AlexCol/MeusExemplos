using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.model;
using InjeçãoDinamica.Properties.src.repositories;
using InjeçãoDinamica.Properties.src.services;
using Microsoft.AspNetCore.Mvc;

namespace InjeçãoDinamica.Properties.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase {
  private readonly IUsuarioService _service;

  public UsuarioController(IUsuarioService service) {
    _service = service;
  }

  [HttpGet]
  public IActionResult GenericUsuario() {
    return Ok(new { mensagem = _service.SayGeneric() });
  }
}
