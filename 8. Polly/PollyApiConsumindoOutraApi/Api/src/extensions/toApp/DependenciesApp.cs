using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.extensions.toApp;

public static class DependenciesApp {
  public static void AddDependencies(this WebApplication app) {
    //!adicionando middlewares
    //app.AddMiddlewares();

    //!adicionando configurações padrão    
    //app.UseHttpsRedirection();
    app.MapControllers();

    app.addSwagger();
  }
}
