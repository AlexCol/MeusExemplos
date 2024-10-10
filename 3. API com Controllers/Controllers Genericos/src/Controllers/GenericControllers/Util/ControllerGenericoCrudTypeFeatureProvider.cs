using System.Reflection;
using Controllers_Genericos.src.Attibutes.GenericController;
using Controllers_Genericos.src.Controllers.GenericControllers.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Controllers_Genericos.src.Controllers.GenericControllers.Util;

/// <summary>
/// Classe que fornece dinamicamente tipos de controladores genéricos à aplicação.
/// </summary>
public class ControllerGenericoCrudTypeFeatureProvider : IApplicationFeatureProvider<ControllerFeature> {

  /// <summary>
  /// Popula o recurso de controlador com controladores genéricos de CRUD e Browse, baseando-se em atributos.
  /// </summary>
  /// <param name="parts">Partes da aplicação ASP.NET Core.</param>
  /// <param name="feature">O recurso de controlador a ser populado.</param>
  public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature) {

    // Obtém o assembly atual, onde esta classe está localizada.
    var currentAssembly = typeof(ControllerGenericoCrudTypeFeatureProvider).Assembly;

    // Busca todos os tipos no assembly atual que possuem o atributo 'GenericControllerCrudAttribute'.
    var candidatesCrud = currentAssembly.GetExportedTypes()
        .Where(x => x.GetCustomAttributes<GenericControllerCrudAttribute>().Any());

    // Para cada candidato encontrado com o atributo CRUD, adiciona um controlador genérico de CRUD.
    foreach (var candidate in candidatesCrud) {
      var typeInfo = typeof(ControllerGenericoCrud<>) // Define o tipo genérico de CRUD.
                      .MakeGenericType(candidate) // Especifica o tipo para o controlador genérico.
                      .GetTypeInfo(); // Obtém informações de tipo para adicionar ao recurso.
      feature.Controllers.Add(typeInfo);
    }

    // Busca todos os tipos no assembly atual que possuem o atributo 'GenericControllerBrowseAttribute'.
    var candidatesBrowse = currentAssembly.GetExportedTypes()
        .Where(x => x.GetCustomAttributes<GenericControllerBrowseAttribute>().Any());

    // Para cada candidato encontrado com o atributo Browse, adiciona um controlador genérico de Browse.
    foreach (var candidate in candidatesBrowse) {
      var typeInfo = typeof(ControllerGenericoBrowse<>) // Define o tipo genérico de Browse.
                      .MakeGenericType(candidate) // Especifica o tipo para o controlador genérico.
                      .GetTypeInfo(); // Obtém informações de tipo para adicionar ao recurso.
      feature.Controllers.Add(typeInfo);
    }
  }
}
