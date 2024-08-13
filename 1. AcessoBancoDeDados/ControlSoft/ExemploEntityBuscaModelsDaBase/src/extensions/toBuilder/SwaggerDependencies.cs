using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityBuscaModelsDaBase.src.extensions.toBuilder;

public static class SwaggerDependencies {
  public static void AddSwagger(this WebApplicationBuilder builder) {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
  }
}
