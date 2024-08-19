using System.Reflection;
using Controllers_Genericos.src.Attibutes.GenericController;
using Controllers_Genericos.src.Controllers.GenericControllers.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Controllers_Genericos.src.Controllers.GenericControllers.Util;

public class ControllerGenericoCrudTypeFeatureProvider : IApplicationFeatureProvider<ControllerFeature> {
  public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature) {
    var currentAssembly = typeof(ControllerGenericoCrudTypeFeatureProvider).Assembly;

    var candidatesCrud = currentAssembly.GetExportedTypes()
        .Where(x => x.GetCustomAttributes<GenericControllerCrudAttribute>().Any());

    foreach (var candidate in candidatesCrud) {
      feature.Controllers.Add(
          typeof(ControllerGenericoCrud<>)
          .MakeGenericType(candidate)
          .GetTypeInfo());
    }

    var candidatesBrowse = currentAssembly.GetExportedTypes()
        .Where(x => x.GetCustomAttributes<GenericControllerBrowseAttribute>().Any());

    foreach (var candidate in candidatesBrowse) {
      feature.Controllers.Add(
          typeof(ControllerGenericoBrowse<>)
          .MakeGenericType(candidate)
          .GetTypeInfo());
    }
  }
}