using ExemploEntityFrameworkWebApi.src.models.error;
using Newtonsoft.Json;
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.middlewares;

public class ExceptionHandlingMiddleware {
  private readonly RequestDelegate _next;

  public ExceptionHandlingMiddleware(RequestDelegate next) {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context) {
    try {
      await _next(context);
    } catch (Exception ex) {
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      context.Response.ContentType = "application/json";

      var error = new ErrorModel(ex);
      var result = JsonConvert.SerializeObject(error);
      await context.Response.WriteAsync(result);

      Log
      .ForContext("SourceContext", typeof(ExceptionHandlingMiddleware).Name) //com isso, é mapeada a classe ao gerar o log e colocar o SourceContext
      .Error($"Ocorreu um erro em: {context.Request.Path}. Erro: {error}");
    }
  }
}
