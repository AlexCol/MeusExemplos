using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.repository;

namespace ExemploEntityFrameworkWebApi.src.services;

public interface IPersonService {
  Task<Person> FindById(int id);
  Task<List<Person>> FindAll();
  Task<List<Person>> FindByName(string name);
  Task<List<Person>> FindByBirth(DateTime birth);
  Task<List<Person>> FindByGender(int genderId);
  Task<Person> Create(Person person);
  Task<Person> Update(Person person);
  Task DeleteById(int id);
}

public class PersonService : IPersonService {
  private readonly IPersonRepository _repository;

  public PersonService(IPersonRepository repository) {
    _repository = repository;
  }

  public async Task<List<Person>> FindAll() {
    return await _repository.FindAll();
  }

  public async Task<Person> FindById(int id) {
    return await _repository.FindById(id);
  }

  public async Task<List<Person>> FindByName(string name) {
    return await _repository.FindByName(name);
  }

  public async Task<List<Person>> FindByBirth(DateTime birth) {
    return await _repository.FindByBirth(birth);
  }

  public async Task<List<Person>> FindByGender(int genderId) {
    return await _repository.FindByGender(genderId);
  }

  public async Task<Person> Create(Person person) {
    if (person.Gender == null || person.Gender.Id <= 0) throw new Exception("Service - Gênero não especificado.");
    return await _repository.Create(person);
  }

  public async Task<Person> Update(Person person) {
    if (person.Gender == null || person.Gender.Id <= 0) throw new Exception("Service - Gênero não especificado.");
    return await _repository.Update(person);
  }

  public async Task DeleteById(int id) {
    await _repository.Delete(id);
  }
}
