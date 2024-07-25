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

  public List<string> ErrorMessage { get; set; } = new List<string>();

  public override string ToString() {
    string error = "";
    foreach (var err in ErrorMessage) {
      error += (error == "") ? err : $";{err}";
    }
    return error;
  }
}
