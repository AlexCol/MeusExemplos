using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExemploEntityFrameworkWebApi.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class MistoController : ControllerBase {
  //controller para montar combinações de dados
  //usar dinamic para retornar, por exemplo, Persons anexando arrays de seus Addresses
}
