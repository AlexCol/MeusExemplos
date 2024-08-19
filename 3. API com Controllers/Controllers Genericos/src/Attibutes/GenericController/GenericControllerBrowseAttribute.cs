using System;

namespace Controllers_Genericos.src.Attibutes.GenericController;
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GenericControllerBrowseAttribute : Attribute {
  public string? Route { get; }

  public GenericControllerBrowseAttribute(string? route = null) {
    Route = route;
  }
}