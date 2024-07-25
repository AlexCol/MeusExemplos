using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public interface IGenericRepository<T> where T : _BaseEntityWithId {
  Task<T> FindById(int id);
  Task<List<T>> FindAll();
  Task<T> Create(T item);
  Task<T> Update(T item);
  Task Delete(int id);
}

public class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {
  private readonly MySqlContext _context;
  private DbSet<T> dataset;

  public GenericRepository(MySqlContext context) {
    _context = context;
    dataset = context.Set<T>();
  }

  virtual public async Task<T> FindById(int id) {
    T item = await dataset.SingleOrDefaultAsync(p => p.Id.Equals(id));
    return item;
  }

  virtual public async Task<List<T>> FindAll() {
    return await dataset.ToListAsync();
  }

  virtual public async Task<T> Create(T item) {
    try {
      await dataset.AddAsync(item);
      await _context.SaveChangesAsync();
      return item;
    } catch (Exception e) {
      throw new InvalidDataException("Erro ao cadastrar. " + e.Message);
    }
  }

  virtual public async Task<T> Update(T item) {
    T itemAtual = await dataset.SingleOrDefaultAsync(p => p.Id.Equals(item.Id));
    if (itemAtual == null) throw new InvalidDataException("Erro ao atualizar o registro.");

    _context.Entry(itemAtual).CurrentValues.SetValues(item);
    await _context.SaveChangesAsync();
    return item;
  }

  virtual public async Task Delete(int id) {
    var user = await FindById(id);
    if (user == null) throw new Exception("Item não encontrado ou já excluído.");

    dataset.Remove(user);
    await _context.SaveChangesAsync();
  }
}
