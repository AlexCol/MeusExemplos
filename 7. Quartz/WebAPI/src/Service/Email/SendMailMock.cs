using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Serilog;

namespace WebAPI.src.Service.Email;

public class SendMailMock : IJob {
    public Task Execute(IJobExecutionContext context) {
        Log.Information("Email sended");
        return Task.CompletedTask;
    }
}
