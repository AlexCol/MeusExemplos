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
public class AddressController : ControllerBase {
  private readonly IGenericService<Address> _service;

  public AddressController(IGenericService<Address> service) {
    _service = service;
  }

  // [HttpPost("create")]
  // public async Task<IActionResult> Create([FromBody] Address newAddress) {
  //   await _service.Create(newAddress);
  //   return Created();
  // }
}
