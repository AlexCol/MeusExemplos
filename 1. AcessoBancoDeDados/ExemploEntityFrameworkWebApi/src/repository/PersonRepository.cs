using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using ExemploEntityFrameworkWebApi.src.repository.Generic;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.repository;

public interface IPersonRepository : IGenericRepository<Person> {
  Task<List<Person>> FindByName(string name);
  Task<List<Person>> FindByBirth(DateTime birth);
  Task<List<Person>> FindByGender(int genderId);
}

public class PersonRepository : GenericRepository<Person>, IPersonRepository {
  public IServiceProvider _service;

  public PersonRepository(MyDBContext context, IServiceProvider service) : base(context, service) {
    _service = service;
  }
  public async Task<List<Person>> FindByName(string name) {
    var query = PrepareQuery();
    var persons = await query.Where(p => p.First_Name.ToLower().Contains(name.ToLower())).ToListAsync();
    return persons;
  }

  public async Task<List<Person>> FindByBirth(DateTime birth) {
    var query = PrepareQuery();
    var persons = await query.Where(p => p.DateOfBirth.Date == birth.Date).ToListAsync();
    return persons;
  }

  public async Task<List<Person>> FindByGender(int genderId) {
    var query = PrepareQuery();
    var persons = await query.Where(p => p.Gender.Id == genderId).ToListAsync();
    return persons;
  }
}

