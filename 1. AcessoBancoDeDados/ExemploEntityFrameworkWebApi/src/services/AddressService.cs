using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.repository;

namespace ExemploEntityFrameworkWebApi.src.services;

public interface IAddressService {
  // Task<Address> FindById(int id);
  // Task<List<Address>> ListByPersonId(int personId);
  Task<Address> Create(Address newAddress);
}

public class AddressService : IAddressService {
  private readonly IAddressRepository _repository;

  public AddressService(IAddressRepository repository) {
    _repository = repository;
  }

  public async Task<Address> Create(Address newAddress) {
    if (newAddress.Person == null || newAddress.Person.Id <= 0) throw new Exception("Service - Pessoa não especificada.");
    return await _repository.Create(newAddress);
  }
}
