using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityFrameworkWebApi.src.extensions.toBuilder;

public static class DependenciesBuilder {
  public static void addDependencies(this WebApplicationBuilder builder) {
    //!adicionando configurações padrão
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers();

    //!adicionando configurações
    builder.addSwagger();
    builder.addMySqlConfig();

    //!adicionando classes para injeções de dependencia

  }
}
