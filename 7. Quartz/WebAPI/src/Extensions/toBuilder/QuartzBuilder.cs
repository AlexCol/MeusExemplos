using Quartz;
using WebAPI.src.Service.Email;
using WebAPI.src.Service.Stuff;

namespace WebAPI.src.Extensions.toBuilder;

public static class QuartzBuilder {
    public static void AddQuartz(this WebApplicationBuilder builder) {
        builder.Services.AddQuartz(confg => {
            //add jobs
            confg.AddJob<SendMailMock>(c => c.WithIdentity("SendMail"));
            confg.AddJob<CheckStuff>(c => c.WithIdentity("CheckStuff"));

            //add triggers
            confg.AddTrigger(t => t
                .ForJob("SendMail")
                .WithIdentity("SendMailTrigger")
                .WithCronSchedule("0/5 * * * * ?") // Schedule para rodar a cada 5 segundos, por exemplo                
            );

            confg.AddTrigger(t => t
                .ForJob("CheckStuff")
                .WithIdentity("CheckStuffTrigger")
                .WithCronSchedule("30/3 * * * * ?") // Schedule para rodar a cada 3 segundos a partir do segundo 30, por exemplo                
            );
        });

        // Configura o Quartz.NET Hosted Service
        builder.Services.AddQuartzHostedService(options => {
            // Configurações do serviço Quartz.NET
            options.WaitForJobsToComplete = true;
        });
    }
}
