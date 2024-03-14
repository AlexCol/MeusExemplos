using Quartz;
using Serilog;

namespace WebAPI.src.Service.Stuff;

public class CheckStuff : IJob {
    public Task Execute(IJobExecutionContext context) {
        Log.Warning("Checking stuff");
        return Task.CompletedTask;
    }
}
