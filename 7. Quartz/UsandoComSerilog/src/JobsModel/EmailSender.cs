
using Quartz;
using UsandoComSerilog.src.Log;

namespace UsandoComSerilog.src.JobsModel {
    public class EmailSender : IJob {
        public Task Execute(IJobExecutionContext context) {
            Logger.Log.Information("Email enviado");
            return Task.CompletedTask;
        }
    }
}