using Serilog;

namespace UsandoComSerilog.src.Log {
    public static class Logger {
        public static ILogger Log { get; } = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/logs.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();
    }
}


/*
optins
restrictedToMinimumLevel = Serilog.Events.LogEventLevel.Verbose, 
string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", 
IFormatProvider? formatProvider = null, 
long? fileSizeLimitBytes = 1073741824, 
Serilog.Core.LoggingLevelSwitch? levelSwitch = null, 
bool buffered = false, 
bool shared = false, 
TimeSpan? flushToDiskInterval = null, 
RollingInterval rollingInterval = RollingInterval.Infinite, 
bool rollOnFileSizeLimit = false, 
int? retainedFileCountLimit = 31, 
System.Text.Encoding? encoding = null, 
Serilog.Sinks.File.FileLifecycleHooks? hooks = null, 
TimeSpan? retainedFileTimeLimit = null)
*/