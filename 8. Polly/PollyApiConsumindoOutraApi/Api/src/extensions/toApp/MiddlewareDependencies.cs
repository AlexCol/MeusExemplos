using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.middlewares;

namespace Api.src.extensions.toApp;

public static class MiddlewareDependencies {
  public static void AddMiddlewares(this WebApplication app) {
    //!adicionando middlewares, importante pois a ordem em que forem declarados, serão executados
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<LogMiddleware>();
  }
}
