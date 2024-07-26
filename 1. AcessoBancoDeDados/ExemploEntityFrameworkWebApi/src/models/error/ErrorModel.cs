using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityFrameworkWebApi.src.models.error;

public class ErrorModel {
  public ErrorModel(string errorMessage) {
    var errors = errorMessage.Split(";");
    foreach (var error in errors) {
      ErrorMessage.Add(error);
    }
  }

  public ErrorModel(IEnumerable<string> errorMessages) {
    ErrorMessage.AddRange(errorMessages);
  }

  public ErrorModel(Exception exception) {
    ErrorMessage.Add(exception.Message);
    var inner = exception.InnerException;
    while (inner != null) {
      ErrorMessage.Add(inner.Message);
      inner = inner.InnerException;
    }
  }

  public List<string> ErrorMessage { get; set; } = new List<string>();

  public override string ToString() {
    string error = "";
    foreach (var err in ErrorMessage) {
      error += (error == "") ? err : $";{err}";
    }
    return error;
  }
}
