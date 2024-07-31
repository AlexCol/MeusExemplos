using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.repository;
using ExemploEntityFrameworkWebApi.src.services.Generic;

namespace ExemploEntityFrameworkWebApi.src.services;

public interface IPersonService : IGenericService<Person> {
}

public class PersonService : GenericService<Person>, IPersonService {
  private readonly IPersonRepository _repository;

  public PersonService(IPersonRepository repository) : base(repository) {
    _repository = repository;
  }
}
