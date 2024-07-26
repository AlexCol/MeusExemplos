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

      var result = JsonConvert.SerializeObject(new ErrorModel(ex));
      await context.Response.WriteAsync(result);
    }
  }
}
