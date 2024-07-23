using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityFrameworkWebApi.src.extensions.toApp;

public static class DependenciesApp {
  public static void addDependencies(this WebApplication app) {
    //!adicionando configurações padrão    
    //app.UseHttpsRedirection();
    app.MapControllers();

    app.addSwagger();
  }
}
