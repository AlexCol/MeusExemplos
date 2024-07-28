using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using ExemploEntityFrameworkWebApi.src.repository.Generic;

namespace ExemploEntityFrameworkWebApi.src.repository;

public interface IAddressRepository : IGenericRepository<Address> {
  //Task<List<Person>> FindByPersonId(string name);
}

public class AddressRepository : GenericRepository<Address>, IAddressRepository {
  public IServiceProvider _service;

  public AddressRepository(MyDBContext context, IServiceProvider service) : base(context, service) {
    _service = service;
  }

  public override async Task<Address> Create(Address newAddress) {
    var person = await _service.GetRequiredService<IGenericRepository<Person>>().FindById(newAddress.Person.Id);
    if (person == null) throw new Exception("Repositorio - Pessoa não encontrada.");

    return await base.Create(newAddress);
  }

}

