
using Serilog;
using Serilog.Events;

namespace ExemploEntityBuscaModelsDaBase.src.extensions.toBuilder;

public static class LogBuilder {
  public static void AddLogConfig(this WebApplicationBuilder builder) {
    //!ativando serilog
    //builder.Host.UseSerilog(Log.Logger);
    builder.Host.UseSerilog((context, configuration) => {
      configuration.MinimumLevel.Information();
      configuration.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
      configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
      configuration.Enrich.FromLogContext();
      configuration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} - {SourceContext}{NewLine}{Exception}"); //SourceContext diz quem gerou
    });
  }
}
