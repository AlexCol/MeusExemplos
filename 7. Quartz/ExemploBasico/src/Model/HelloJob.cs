using Quartz;

namespace ExemploBasico {
    public class HelloJob : IJob {
        public async Task Execute(IJobExecutionContext context) {
            await Console.Out.WriteLineAsync("HelloJob is executing.");
        }
    }
}