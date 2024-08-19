using System;

namespace Controllers_Genericos.src.Attibutes.GenericController;
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GenericControllerCrudAttribute : Attribute {
  public string? Route { get; }

  public GenericControllerCrudAttribute(string? route = null) {
    Route = route;
  }
}
