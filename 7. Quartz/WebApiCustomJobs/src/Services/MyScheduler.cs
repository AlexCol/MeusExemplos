using Quartz;
using Quartz.Impl.Matchers;
using WebApiCustomJobs.src.Model;

namespace WebApiCustomJobs.src.Services;

public interface IMyScheduler {
  Task<string> StartJob(JobRequest jobRequest);
  Task<string> StopJob(string jobName);
  Task<List<string>> GetRunningJobs();
}

public class MyScheduler : IMyScheduler {
  private readonly ISchedulerFactory _schedulerFactory;

  public MyScheduler(ISchedulerFactory schedulerFactory) {
    _schedulerFactory = schedulerFactory;
  }

  public async Task<string> StartJob(JobRequest jobRequest) {
    var scheduler = await _schedulerFactory.GetScheduler();

    var job = JobBuilder.Create<EmailSender>()
        .WithIdentity(jobRequest.Email)
        .UsingJobData("Email", jobRequest.Email)
        .UsingJobData("Interval", jobRequest.Interval)
        .Build();

    var trigger = TriggerBuilder.Create()
        .WithIdentity($"{jobRequest.Email}-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(jobRequest.Interval).RepeatForever())
        .Build();

    await scheduler.ScheduleJob(job, trigger);
    await scheduler.Start();

    return $"Job started for email: {jobRequest.Email} with interval: {jobRequest.Interval} seconds";
  }

  public async Task<string> StopJob(string jobName) {
    var scheduler = await _schedulerFactory.GetScheduler();
    var deleted = await scheduler.DeleteJob(new JobKey(jobName));
    return deleted ? $"Job stopped for email: {jobName}" : $"Job not founded!";
  }

  public async Task<List<string>> GetRunningJobs() {
    var scheduler = await _schedulerFactory.GetScheduler();
    var list = new List<string>();

    var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
    foreach (var key in jobKeys) {
      var job = await scheduler.GetJobDetail(key);
      list.Add(job.Key.Name);
    }

    return list;
  }
}
