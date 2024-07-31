using System.Reflection;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public partial class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {
  /*
    ! Este método deve ser chamado antes de await _context.SaveChangesAsync(); pois esse comando salva
    ! apenas dados simples (string, int, etc), e não as relações. Se apenas SaveChangesAsync for usado,
    ! e a classe tiver uma relação (como Person ter Gender), essa relação não será atualizada.
    ! Com isso, se usar await UpdateRelatedEntities() antes de SaveChangesAsync, será garantido que 
    ! a relação será buscada e corretamente vinculada à entidade que será salva, desde que a 
    ! relação também seja do tipo _BaseEntityWithId.
  */
  private async Task UpdateRelatedEntities(T newItem) { // Atualiza as entidades relacionadas de um item novo usando o mesmo item como base
    await UpdateRelatedEntities(newItem, newItem); // Chama a versão sobrecarregada do método passando o item novo como existente também
  }

  private async Task UpdateRelatedEntities(T existingItem, T newItem) { // Atualiza as entidades relacionadas entre o item existente e o novo item
    var navigationProperties = GetNavigationProperties(); // Obtém todas as propriedades de navegação do tipo T que são do tipo _BaseEntityWithId

    foreach (var property in navigationProperties) { // Itera sobre cada propriedade de navegação
      await UpdateNavigationProperty(existingItem, newItem, property); // Atualiza a propriedade de navegação específica
    }
  }

  private IEnumerable<PropertyInfo> GetNavigationProperties() { // Obtém as propriedades de navegação do tipo T que são do tipo _BaseEntityWithId
    return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance) // Obtém todas as propriedades públicas e de instância
        .Where(p => typeof(_BaseEntityWithId).IsAssignableFrom(p.PropertyType)); // Filtra propriedades do tipo _BaseEntityWithId
  }

  private async Task UpdateNavigationProperty(T existingItem, T newItem, PropertyInfo property) { // Atualiza uma propriedade de navegação específica do item existente com base no novo item
    var newValue = property.GetValue(newItem); // Obtém o valor da propriedade no novo item

    if (newValue != null) { // Se a nova propriedade não for nula
      var newEntity = newValue as _BaseEntityWithId; // Tenta converter o valor da nova propriedade para _BaseEntityWithId
      if (newEntity != null) {
        var currentEntity = await GetCurrentValueOfProperty(newEntity); // Obtém a entidade atual correspondente ao novo valor

        if (currentEntity == null) { // Se a entidade atual não for encontrada, lança uma exceção
          throw new Exception($"Falha ao registrar nova: {newItem.GetType().Name}. Não encontrado código {newEntity.Id} para {newEntity.GetType().Name} para vincular.");
        }

        property.SetValue(existingItem, currentEntity); // Define o valor da propriedade no item existente com a entidade atualizada
      }
    }
  }

  // Obtém o valor da propriedade atual usando o repositório para buscar a entidade pelo ID
  private async Task<object> GetCurrentValueOfProperty(_BaseEntityWithId newProperty) {
    var repositoryType = typeof(IGenericRepository<>).MakeGenericType(newProperty.GetType()); // Obtém o tipo do repositório genérico para o tipo da nova propriedade
    var repository = _service.GetRequiredService(repositoryType); // Obtém o repositório genérico do serviço de injeção de dependência
    var method = repositoryType.GetMethod("FindById"); // Obtém o método FindById do repositório

    var parameters = new object[] { newProperty.Id }; // Prepara os parâmetros para o método FindById (neste caso, o ID da nova entidade)
    var task = (Task)method.Invoke(repository, parameters);
    await task.ConfigureAwait(false); // Invoca o método FindById de forma assíncrona
    var resultProperty = task.GetType().GetProperty("Result"); // Obtém a propriedade Result da task, que contém a entidade encontrada
    return resultProperty.GetValue(task); // Retorna o valor da entidade encontrada
  }
}