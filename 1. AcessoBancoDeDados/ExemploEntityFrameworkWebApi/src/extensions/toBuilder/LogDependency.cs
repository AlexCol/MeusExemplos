
using Serilog;

namespace ExemploEntityFrameworkWebApi.src.extensions.toBuilder;

public static class LogBuilder {
  public static void AddLogConfig(this WebApplicationBuilder builder) {
    //!ativando serilog
    builder.Host.UseSerilog((context, configuration) => configuration.WriteTo.Console());
  }
}
