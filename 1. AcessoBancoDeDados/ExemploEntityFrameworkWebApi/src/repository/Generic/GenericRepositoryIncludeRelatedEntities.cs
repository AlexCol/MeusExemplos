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
    return IncludeRelatedEntities(_dataset, new HashSet<string>()); // Chama o método IncludeRelatedEntities com um HashSet para rastrear entidades incluídas
  }

  private IQueryable<T> IncludeRelatedEntities(IQueryable<T> query, HashSet<string> includedProperties) { // Método privado que inclui as entidades relacionadas na consulta
    var navigationProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance) // Obtém todas as propriedades de navegação do tipo T que são do tipo _BaseEntityWithId
                                       .Where(p => typeof(_BaseEntityWithId).IsAssignableFrom(p.PropertyType)
                                       //|| (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                                       //! devido a problemas, optador por não trazer referencia de collections no caso de consulta
                                       );

    foreach (var property in navigationProperties) { // Percorre cada propriedade de navegação encontrada
      var propertyName = property.Name;
      if (!includedProperties.Contains(propertyName)) { // Verifica se a propriedade já foi incluída
        includedProperties.Add(propertyName); // Adiciona a propriedade ao HashSet
        query = query.Include(propertyName); // Inclui a propriedade de navegação na consulta usando o método Include do Entity Framework

        // Inclui propriedades de navegação aninhadas
        var propertyType = property.PropertyType;
        if (typeof(_BaseEntityWithId).IsAssignableFrom(propertyType)) { // Verifica se a propriedade é do tipo _BaseEntityWithId
          var includePath = $"{propertyName}";
          query = IncludeNestedRelatedEntities(query, propertyType, includePath, includedProperties); // Chama o método IncludeNestedRelatedEntities para incluir propriedades aninhadas
        }
      }
    }
    return query; // Retorna a consulta atualizada com as entidades relacionadas incluídas
  }

  private IQueryable<T> IncludeNestedRelatedEntities(IQueryable<T> query, Type entityType, string parentPath, HashSet<string> includedProperties) { // Método privado que inclui propriedades de navegação aninhadas
    var navigationProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance) // Obtém todas as propriedades de navegação do tipo entityType
                                         .Where(p => typeof(_BaseEntityWithId).IsAssignableFrom(p.PropertyType)
                                         //|| (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                                         //! devido a problemas, optador por não trazer referencia de collections no caso de consulta
                                         );

    foreach (var property in navigationProperties) { // Percorre cada propriedade de navegação encontrada
      var propertyName = $"{parentPath}.{property.Name}";
      if (!includedProperties.Contains(propertyName)) { // Verifica se a propriedade já foi incluída
        includedProperties.Add(propertyName); // Adiciona a propriedade ao HashSet
        query = query.Include(propertyName); // Inclui a propriedade de navegação na consulta usando o método Include do Entity Framework

        // Inclui propriedades de navegação aninhadas recursivamente
        var propertyType = property.PropertyType;
        if (typeof(_BaseEntityWithId).IsAssignableFrom(propertyType)) { // Verifica se a propriedade é do tipo _BaseEntityWithId
          query = IncludeNestedRelatedEntities(query, propertyType, propertyName, includedProperties); // Chama o método IncludeNestedRelatedEntities para incluir propriedades aninhadas
        }
      }
    }
    return query; // Retorna a consulta atualizada com as entidades relacionadas incluídas
  }
}
