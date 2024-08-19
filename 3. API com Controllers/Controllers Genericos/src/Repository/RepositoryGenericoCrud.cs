using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers_Genericos.src.Repository;

public interface IRepositoryGenericoCrud<T> where T : class {
  T Get(int id);
  IEnumerable<T> GetAll();
  long Insert(T entity);
  bool Update(T entity);
  bool Delete(int id);
}
public class RepositoryGenericoCrud<T> : IRepositoryGenericoCrud<T> where T : class {
  private readonly List<T> _storage = new List<T>();

  public T Get(int id) {
    var obj = _storage.ElementAtOrDefault(id);
    if (obj == null) {
      throw new Exception("Object not founded!");
    }
    return obj;
  }

  public IEnumerable<T> GetAll() => _storage;
  public long Insert(T entity) {
    _storage.Add(entity);
    return _storage.Count - 1;
  }
  public bool Update(T entity) {
    // Simulação de atualização
    return true;
  }
  public bool Delete(int id) {
    if (id >= 0 && id < _storage.Count) {
      _storage.RemoveAt(id);
      return true;
    }
    return false;
  }
}
