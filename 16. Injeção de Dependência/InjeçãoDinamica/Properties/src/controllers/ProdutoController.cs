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
public class ProdutoController : ControllerBase {
  private readonly IProdutoService _service;

  public ProdutoController(IProdutoService service) {
    _service = service;
  }

  [HttpGet]
  public IActionResult GenericProduto() {
    return Ok(new { mensagem = _service.SayGeneric() });
  }
}
