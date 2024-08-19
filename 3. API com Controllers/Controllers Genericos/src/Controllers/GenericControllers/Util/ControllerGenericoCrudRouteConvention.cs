using System.Reflection;
using Controllers_Genericos.src.Attibutes.GenericController;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Controllers_Genericos.src.Controllers.GenericControllers.Util;

public class ControllerGenericoCrudRouteConvention : IControllerModelConvention {
  public void Apply(ControllerModel controller) {
    if (controller.ControllerType.IsGenericType) {
      var genericType = controller.ControllerType.GenericTypeArguments[0];

      var customCrudAttribute = genericType.GetCustomAttribute<GenericControllerCrudAttribute>();
      if (customCrudAttribute?.Route != null) {
        var route = customCrudAttribute.Route;
        AddRoute(controller, route);
      }

      var customBrowseAttribute = genericType.GetCustomAttribute<GenericControllerBrowseAttribute>();
      if (customBrowseAttribute?.Route != null) {
        var route = customBrowseAttribute.Route;
        AddRoute(controller, route);
      }
    }
  }

  private void AddRoute(ControllerModel controller, string route) {
    var genericType = controller.ControllerType.GenericTypeArguments[0];
    route = route.Replace("[controller]", genericType.Name);

    controller.Selectors.Add(new SelectorModel {
      AttributeRouteModel = new AttributeRouteModel(new Microsoft.AspNetCore.Mvc.RouteAttribute(route)),
    });
  }
}