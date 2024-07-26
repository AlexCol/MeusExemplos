using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public interface IPersonRepository : IGenericRepository<Person> {
  Task<List<Person>> FindByName(string name);
  Task<List<Person>> FindByBirth(DateTime birth);
  Task<List<Person>> FindByGender(int genderId);
}

public class PersonRepository : GenericRepository<Person>, IPersonRepository {
  public IServiceProvider _service;

  public PersonRepository(MyDBContext context, IServiceProvider service) : base(context) {
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

  public override async Task<Person> Create(Person newPerson) {
    var gender = await _service.GetRequiredService<IGenericRepository<Gender>>().FindById(newPerson.Gender.Id);
    if (gender == null) throw new Exception("Repositorio - Gênero inválido.");
    newPerson.Gender = gender;
    return await base.Create(newPerson);
  }

  public override async Task<Person> Update(Person person) {
    var gender = await _service.GetRequiredService<IGenericRepository<Gender>>().FindById(person.Gender.Id);
    if (gender == null) throw new Exception("Repositorio - Gênero inválido.");
    person.Gender = gender;
    return await base.Update(person);
  }
}

