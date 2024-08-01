using System.Reflection;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using ExemploEntityFrameworkWebApi.src.models.search;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public interface IGenericRepository<T> where T : _BaseEntityWithId {
  Task<T> FindById(int id);
  Task<List<T>> FindAll();
  Task<List<T>> SeachByCriteria(List<SearchCriteria> properties);
  Task<T> Create(T registro);
  Task<T> Update(T registro);
  Task DeleteById(int id);
}

//! deixado como partcial, para deixar a parte mais complexa, com reflections, em outro arquivo
//! deixando aqui a parte simples, para ficar mais facil de tentar entender o que ocorre
//! na reflection
public partial class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {

  private readonly MyDBContext _context;
  private readonly IServiceProvider _service;
  private DbSet<T> _dataset;

  public GenericRepository(MyDBContext context, IServiceProvider service) {
    _context = context;
    _service = service;
    _dataset = context.Set<T>();
  }

  public virtual async Task<T> FindById(int id) {
    var query = PrepareQuery().SingleOrDefaultAsync(p => p.Id == id);
    return await query;
  }

  public virtual async Task<List<T>> FindAll() {
    return await PrepareQuery().ToListAsync();
  }

  public async Task<List<T>> SeachByCriteria(List<SearchCriteria> properties) {
    return await FindByPropertiesAsync(properties); //deixado aqui pra manter tudo que é da interce num arquivo só
  }

  public virtual async Task<T> Create(T registro) {
    try {
      await UpdateRelatedEntities(registro);
      await _dataset.AddAsync(registro);
      await _context.SaveChangesAsync();
      return registro;
    } catch (Exception e) {
      throw new InvalidDataException("Erro ao cadastrar. " + e.Message);
    }
  }

  public virtual async Task<T> Update(T registro) {
    T registroAtual = await FindById(registro.Id);
    if (registroAtual == null) throw new InvalidDataException($"Repositório - Erro ao atualizar {typeof(T).Name}. Não encontrado com id {registro.Id}.");

    _context.Entry(registroAtual).CurrentValues.SetValues(registro);
    await UpdateRelatedEntities(registroAtual, registro);

    await _context.SaveChangesAsync();
    return registro;
  }

  public virtual async Task DeleteById(int id) {
    var registro = await FindById(id);
    if (registro == null) throw new Exception("Item não encontrado ou já excluído.");

    _dataset.Remove(registro);
    await _context.SaveChangesAsync();
  }
}