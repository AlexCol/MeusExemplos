using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityBuscaModelsDaBase.src.extensions.toApp;

public static class SwaggerDependencies {
  public static void addSwagger(this WebApplication app) {
    if (app.Environment.IsDevelopment()) {
      app.UseSwagger();
      app.UseSwaggerUI();
    }
  }
}
