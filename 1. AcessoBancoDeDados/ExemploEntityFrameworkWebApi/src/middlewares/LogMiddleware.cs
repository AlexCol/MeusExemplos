
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.middlewares;

public class LogMiddleware {
  private readonly RequestDelegate _next;

  public LogMiddleware(RequestDelegate next) {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context) {

    Log
      .ForContext("SourceContext", typeof(LogMiddleware).Name) //com isso, é mapeada a classe ao gerar o log e colocar o SourceContext
      .Information($"Executando {context.Request.Path}");
    await _next(context);
  }
}
