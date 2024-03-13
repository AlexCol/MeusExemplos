using Serilog;

namespace ComConsole.src;

public static class Logger {
	public static ILogger Log { get; } = new LoggerConfiguration()
			.WriteTo.Console() //!escrever no console
			.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // Gravar em um arquivo
			.CreateLogger();
}
