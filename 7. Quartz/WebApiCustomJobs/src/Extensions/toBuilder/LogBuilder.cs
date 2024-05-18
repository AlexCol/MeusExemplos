using Serilog;

namespace WebApiCustomJobs.src.Extensions.toBuilder;

public static class LogBuilder {
  public static void addLogService(this WebApplicationBuilder builder) {
    //!ativando serilog
    builder.Host.UseSerilog((context, configuration) => {
      configuration.WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day);
      configuration.WriteTo.Console();
    });
  }
}
