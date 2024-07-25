using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public interface IPersonRepository : IGenericRepository<Person> {
  Task<List<Person>> FindByName(string name);
}

public class PersonRepository : GenericRepository<Person>, IPersonRepository {
  public IServiceProvider _service;

  public PersonRepository(MySqlContext context, IServiceProvider service) : base(context) {
    _service = service;
  }

  public async Task<List<Person>> FindByName(string name) {
    var query = PrepareQuery();
    var persons = await query.Where(p => p.First_Name.ToLower().Contains(name.ToLower())).ToListAsync();
    return persons;
  }

  public override async Task<Person> Create(Person newPerson) {
    var gender = await _service.GetRequiredService<IGenericRepository<Gender>>().FindById(newPerson.Gender.Id);
    if (gender == null) throw new Exception("Repositorio - Gênero inválido.");
    newPerson.Gender = gender;
    return await base.Create(newPerson);
  }
}

