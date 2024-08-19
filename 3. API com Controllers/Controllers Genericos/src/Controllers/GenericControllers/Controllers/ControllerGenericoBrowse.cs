using Controllers_Genericos.src.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Controllers_Genericos.src.Controllers.GenericControllers.Controllers;
[ApiController]
public class ControllerGenericoBrowse<T> : ControllerBase where T : class {
  private readonly IRepositoryGenericoCrud<T> _repository;

  public ControllerGenericoBrowse(IRepositoryGenericoCrud<T> repository) {
    _repository = repository;
  }

  [HttpGet("")]
  public IActionResult Browse() {
    return Ok(_repository.GetAll());
  }
}