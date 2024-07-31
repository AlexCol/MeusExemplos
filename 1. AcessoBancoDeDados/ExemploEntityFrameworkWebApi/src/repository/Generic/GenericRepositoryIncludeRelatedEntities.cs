using System.Reflection;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public partial class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {
  /*
    ! O método PrepareQuery é usado para preparar consultas incluindo as entidades relacionadas.
    ! Com ele, as consultas trazem também as entidades relacionadas. Por exemplo, ao buscar uma
    ! entidade Person, ele trará também a entidade Gender associada.
    ! PrepareQuery() deve ser usado nos métodos de busca da GenericRepository.
    ! Se uma classe herdar de GenericRepository, ela também terá acesso ao PrepareQuery() para
    ! garantir que os dados relacionados sejam corretamente incluídos.
  */

  protected IQueryable<T> PrepareQuery() { // Método protegido que prepara a consulta para incluir entidades relacionadas
    return IncludeRelatedEntities(_dataset); // Chama o método IncludeRelatedEntities para incluir as entidades relacionadas na consulta
  }

  private IQueryable<T> IncludeRelatedEntities(IQueryable<T> query) { // Método privado que inclui as entidades relacionadas na consulta
    var navigationProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance) // Obtém todas as propriedades de navegação do tipo T que são do tipo _BaseEntityWithId
                                       .Where(p => typeof(_BaseEntityWithId).IsAssignableFrom(p.PropertyType));

    foreach (var property in navigationProperties) { // Percorre cada propriedade de navegação encontrada      
      query = query.Include(property.Name); // Inclui a propriedade de navegação na consulta usando o método Include do Entity Framework
    }
    return query; // Retorna a consulta atualizada com as entidades relacionadas incluídas
  }
}