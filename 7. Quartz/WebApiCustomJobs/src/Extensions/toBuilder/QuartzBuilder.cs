
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
using WebApiCustomJobs.src.Services;

namespace WebAPI.src.Extensions.toBuilder;

public static class QuartzBuilder {
  public static void AddQuartz(this WebApplicationBuilder builder) {
    builder.Services.AddSingleton<IJobFactory, MicrosoftDependencyInjectionJobFactory>();
    builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
    builder.Services.AddSingleton<EmailSender>();

    builder.Services.AddQuartzHostedService(options => {
      options.WaitForJobsToComplete = true;
    });
  }
}
