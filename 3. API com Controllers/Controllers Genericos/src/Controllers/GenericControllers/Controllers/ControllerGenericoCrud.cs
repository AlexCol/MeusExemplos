using Controllers_Genericos.src.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Controllers_Genericos.src.Controllers.GenericControllers.Controllers;
[ApiController]
public class ControllerGenericoCrud<T> : ControllerBase where T : class {
  private readonly IRepositoryGenericoCrud<T> _repository;

  public ControllerGenericoCrud(IRepositoryGenericoCrud<T> repository) {
    _repository = repository;
  }

  [HttpGet("{id}")]
  public IActionResult Get(int id) {
    var item = _repository.Get(id);
    return item != null ? Ok(item) : NotFound();
  }

  [HttpGet("getall")]
  public IActionResult GetAll() {
    return Ok(_repository.GetAll());
  }

  [HttpPost("")]
  public IActionResult Create(T item) {
    var id = _repository.Insert(item);
    return CreatedAtAction(nameof(Get), new { id }, item);
  }

  [HttpPut("{id}")]
  public IActionResult Update(int id, T item) {
    if (_repository.Update(item))
      return NoContent();
    return NotFound();
  }

  [HttpDelete("{id}")]
  public IActionResult Delete(int id) {
    if (_repository.Delete(id))
      return NoContent();
    return NotFound();
  }
}
