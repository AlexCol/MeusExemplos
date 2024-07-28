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

//! deixado como partcial, para deixar a parte mais complexa, com reflections, em outro arquivo
//! deixando aqui a parte simples, para ficar mais facil de tentar entender o que ocorre
//! na reflection
public partial class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {

  private readonly MyDBContext _context;
  private readonly IServiceProvider _service;
  private DbSet<T> dataset;

  public GenericRepository(MyDBContext context, IServiceProvider service) {
    _context = context;
    _service = service;
    dataset = context.Set<T>();
  }

  public virtual async Task<T> FindById(int id) {
    IQueryable<T> query = dataset;
    query = IncludeRelatedEntities(query);
    T registro = await query.SingleOrDefaultAsync(p => p.Id.Equals(id));
    return registro;
  }

  public virtual async Task<List<T>> FindAll() {
    IQueryable<T> query = dataset;
    query = IncludeRelatedEntities(query);
    return await query.ToListAsync();
  }

  public virtual async Task<T> Create(T registro) {
    try {
      await UpdateRelatedEntities(registro);
      await dataset.AddAsync(registro);
      await _context.SaveChangesAsync();
      return registro;
    } catch (Exception e) {
      throw new InvalidDataException("Erro ao cadastrar. " + e.Message);
    }
  }

  public virtual async Task<T> Update(T registro) {
    T registroAtual = await FindById(registro.Id);
    var type = registro.GetType();
    if (registroAtual == null) throw new InvalidDataException($"Repositório - Erro ao atualizar {type.Name}. Não encontrado com id {registro.Id}.");

    _context.Entry(registroAtual).CurrentValues.SetValues(registro);
    await UpdateRelatedEntities(registroAtual, registro);

    await _context.SaveChangesAsync();
    return registro;
  }

  public virtual async Task Delete(int id) {
    var registro = await FindById(id);
    if (registro == null) throw new Exception("Item não encontrado ou já excluído.");

    dataset.Remove(registro);
    await _context.SaveChangesAsync();
  }
}