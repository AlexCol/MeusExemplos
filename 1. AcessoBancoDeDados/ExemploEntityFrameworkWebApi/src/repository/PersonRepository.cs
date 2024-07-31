using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using ExemploEntityFrameworkWebApi.src.repository.Generic;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.repository;

public interface IPersonRepository : IGenericRepository<Person> { }

public class PersonRepository : GenericRepository<Person>, IPersonRepository {
  public IServiceProvider _service;

  public PersonRepository(MyDBContext context, IServiceProvider service) : base(context, service) {
    _service = service;
  }
}

