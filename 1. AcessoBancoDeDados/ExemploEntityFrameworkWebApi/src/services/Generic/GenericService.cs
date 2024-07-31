using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.repository.Generic;

namespace ExemploEntityFrameworkWebApi.src.services.Generic;

public interface IGenericService<T> where T : _BaseEntityWithId {
  Task<T> FindById(int id);
  Task<List<T>> FindAll();
  Task<List<T>> SeachByCriteria(Dictionary<string, object> properties);
  Task<List<T>> SeachByCriteria(List<Tuple<string, object, object>> properties);
  Task<T> Create(T entity);
  Task<T> Update(T entity);
  Task DeleteById(int id);
}

public class GenericService<T> : IGenericService<T> where T : _BaseEntityWithId {
  private readonly IGenericRepository<T> _repository;

  public GenericService(IGenericRepository<T> repository) {
    _repository = repository;
  }

  public async Task<T> FindById(int id) {
    return await _repository.FindById(id);
  }

  public async Task<List<T>> FindAll() {
    return await _repository.FindAll();
  }

  public async Task<List<T>> SeachByCriteria(Dictionary<string, object> properties) {
    return await _repository.SeachByCriteria(properties);
  }

  public async Task<List<T>> SeachByCriteria(List<Tuple<string, object, object>> properties) {
    return await _repository.SeachByCriteria(properties);
  }

  public async Task<T> Create(T entity) {
    return await _repository.Create(entity);
  }

  public async Task<T> Update(T entity) {
    return await _repository.Update(entity);
  }

  public async Task DeleteById(int id) {
    await _repository.DeleteById(id);
  }
}
