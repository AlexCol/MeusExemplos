using System.Reflection;
using Controllers_Genericos.src.Attibutes.GenericController;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Controllers_Genericos.src.Controllers.GenericControllers.Util;

/// <summary>
/// Classe que implementa uma convenção de rota para controladores genéricos de CRUD.
/// </summary>
public class ControllerGenericoCrudRouteConvention : IControllerModelConvention {

  /// <summary>
  /// Aplica a convenção de roteamento ao modelo do controlador.
  /// </summary>
  /// <param name="controller">O modelo de controlador que receberá a convenção.</param>
  public void Apply(ControllerModel controller) {

    // Verifica se o controlador é do tipo genérico.
    if (controller.ControllerType.IsGenericType) {

      // Obtém o tipo genérico do controlador (por exemplo, 'Produto' em 'Controller<Produto>').
      var genericType = controller.ControllerType.GenericTypeArguments[0];

      // Busca o atributo personalizado 'GenericControllerCrudAttribute' definido na classe genérica.
      var customCrudAttribute = genericType.GetCustomAttribute<GenericControllerCrudAttribute>();

      // Verifica se o atributo existe e possui uma rota definida.
      if (customCrudAttribute?.Route != null) {
        var route = customCrudAttribute.Route; // Pega a rota definida no atributo.
        AddRoute(controller, route); // Chama o método para adicionar essa rota ao controlador.
      }

      // Busca o atributo personalizado 'GenericControllerBrowseAttribute' na classe genérica.
      var customBrowseAttribute = genericType.GetCustomAttribute<GenericControllerBrowseAttribute>();

      // Verifica se o atributo existe e possui uma rota definida.
      if (customBrowseAttribute?.Route != null) {
        var route = customBrowseAttribute.Route; // Pega a rota definida no atributo.
        AddRoute(controller, route); // Adiciona essa rota ao controlador.
      }
    }
  }

  /// <summary>
  /// Adiciona uma nova rota ao controlador, substituindo "[controller]" pelo nome do tipo genérico.
  /// </summary>
  /// <param name="controller">O modelo de controlador que receberá a rota.</param>
  /// <param name="route">A rota a ser aplicada ao controlador.</param>
  private void AddRoute(ControllerModel controller, string route) {
    var genericType = controller.ControllerType.GenericTypeArguments[0]; // Obtém o tipo genérico do controlador.

    // Substitui o placeholder "[controller]" pelo nome do tipo genérico.
    route = route.Replace("[controller]", genericType.Name);

    // Adiciona um novo seletor ao controlador, com a rota configurada.
    controller.Selectors.Add(new SelectorModel {
      AttributeRouteModel = new AttributeRouteModel(new Microsoft.AspNetCore.Mvc.RouteAttribute(route)), // Define a rota como um atributo.
    });
  }
}
