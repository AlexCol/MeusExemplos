using System.Reflection;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public interface IGenericRepository<T> where T : _BaseEntityWithId {
  Task<T> FindById(int id);
  Task<List<T>> FindAll();
  Task<T> Create(T registro);
  Task<T> Update(T registro);
  Task Delete(int id);
}

public class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {
  private readonly MyDBContext _context;
  private DbSet<T> dataset;

  public GenericRepository(MyDBContext context) {
    _context = context;
    dataset = context.Set<T>();
  }

  virtual public async Task<T> FindById(int id) {
    IQueryable<T> query = dataset;
    query = IncludeRelatedEntities(query);
    T registro = await query.SingleOrDefaultAsync(p => p.Id.Equals(id));
    return registro;
  }

  virtual public async Task<List<T>> FindAll() {
    IQueryable<T> query = dataset;
    query = IncludeRelatedEntities(query);
    return await query.ToListAsync();
  }

  virtual public async Task<T> Create(T registro) {
    try {
      await dataset.AddAsync(registro);
      await _context.SaveChangesAsync();
      return registro;
    } catch (Exception e) {
      throw new InvalidDataException("Erro ao cadastrar. " + e.Message);
    }
  }

  virtual public async Task<T> Update(T registro) {
    T registroAtual = await FindById(registro.Id);
    if (registroAtual == null) throw new InvalidDataException("Erro ao atualizar o registro. Não encontrado.");

    _context.Entry(registroAtual).CurrentValues.SetValues(registro);
    UpdateRelatedEntities(registroAtual, registro);

    await _context.SaveChangesAsync();
    return registro;
  }

  virtual public async Task Delete(int id) {
    var registro = await FindById(id);
    if (registro == null) throw new Exception("Item não encontrado ou já excluído.");

    dataset.Remove(registro);
    await _context.SaveChangesAsync();
  }

  //????????????????
  protected IQueryable<T> PrepareQuery() { //! processo para buscar automaticamente todas as classes vinculadas (ex, traz gender se buscar por uma person)
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

  //! _context.Entry(registroAtual).CurrentValues.SetValues(registro); atualiza apenas dados simples, e não suas relações
  //! com isso aqui, após atualizar os dados simples, navega entre as propriedades, atualizando as informações
  //! desde que a relação seja do tipo _BaseEntityWithId
  private void UpdateRelatedEntities(T existingItem, T newItem) {
    var navigationProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => typeof(IEnumerable<>).IsAssignableFrom(p.PropertyType) ||
                    typeof(_BaseEntityWithId).IsAssignableFrom(p.PropertyType));

    foreach (var property in navigationProperties) {
      var newValue = property.GetValue(newItem);
      var currentValue = property.GetValue(existingItem);

      if (newValue != null && newValue is IEnumerable<object> newCollection) {
        var currentCollection = currentValue as IEnumerable<object>;
        if (currentCollection != null) {
          foreach (var newItemInCollection in newCollection) {
            if (!_context.Entry(newItemInCollection).IsKeySet) {
              _context.Attach(newItemInCollection);
            }
          }
        }
      } else if (newValue != null) {
        var newEntity = newValue as _BaseEntityWithId;
        if (newEntity != null && !_context.Entry(newEntity).IsKeySet) {
          _context.Attach(newEntity);
        }
        property.SetValue(existingItem, newValue);
      }
    }
  }
}
