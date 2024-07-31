using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.repository.Generic;

namespace ExemploEntityFrameworkWebApi.src.services;

public interface IGenderService {
  Task<Gender> FindById(int id);
  Task<List<Gender>> FindAll();
  Task<List<Gender>> FindByDescriptionCaseInsentive(string description);
  Task<Gender> Create(Gender gender);
  Task<Gender> Update(Gender gender);
  Task DeleteById(int id);
}

public class GenderService : IGenderService {
  private readonly IGenericRepository<Gender> _repository;

  public GenderService(IGenericRepository<Gender> repository) {
    _repository = repository;
  }

  public async Task<Gender> FindById(int id) {
    return await _repository.FindById(id);
  }

  public async Task<List<Gender>> FindAll() {
    return await _repository.FindAll();
  }

  public async Task<List<Gender>> FindByDescriptionCaseInsentive(string description) {
    return (await _repository.FindAll()).Where(g => g.Description.ToLower().Contains(description.ToLower())).ToList();
  }

  public async Task<Gender> Create(Gender gender) {
    return await _repository.Create(gender);
  }

  public async Task<Gender> Update(Gender gender) {
    if (gender.Id == 0) throw new Exception("Não é possível atualizar registro sem Id!");
    return await _repository.Update(gender);
  }

  public async Task DeleteById(int id) {
    await _repository.DeleteById(id);
  }
}
