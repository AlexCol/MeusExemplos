using System.Reflection;
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
    IQueryable<T> query = dataset;
    query = IncludeRelatedEntities(query);
    T item = await query.SingleOrDefaultAsync(p => p.Id.Equals(id));
    return item;
  }

  virtual public async Task<List<T>> FindAll() {
    IQueryable<T> query = dataset;
    query = IncludeRelatedEntities(query);
    return await query.ToListAsync();
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

  //????????????????? rotina para buscar automaticamente todas as classes vinculadas (ex, traz gender se buscar por uma person)
  protected IQueryable<T> PrepareQuery() {
    IQueryable<T> query = dataset;
    return IncludeRelatedEntities(query); //ver QueribleExtension.cs para detalhes
  }

  private IQueryable<T> IncludeRelatedEntities(IQueryable<T> query) {
    var navigationProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Where(p => typeof(IEnumerable<>).IsAssignableFrom(p.PropertyType) ||
                                                   typeof(_BaseEntityWithId).IsAssignableFrom(p.PropertyType));

    foreach (var property in navigationProperties) {
      query = query.Include(property.Name);
    }

    return query;
  }
}
