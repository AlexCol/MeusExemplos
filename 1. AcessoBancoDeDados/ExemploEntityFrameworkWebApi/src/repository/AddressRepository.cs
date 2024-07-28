using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using ExemploEntityFrameworkWebApi.src.repository.Generic;

namespace ExemploEntityFrameworkWebApi.src.repository;

public interface IAddressRepository : IGenericRepository<Address> {

}

public class AddressRepository : GenericRepository<Address>, IAddressRepository {
  public IServiceProvider _service;

  public AddressRepository(MyDBContext context, IServiceProvider service) : base(context, service) {
    _service = service;
  }
}

