using Microsoft.AspNetCore.Mvc;
using Quartz;
using WebApiCustomJobs.src.Model;
using WebApiCustomJobs.src.Services;

namespace WebApiCustomJobs.src.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase {
  private static List<string> _runningJobs = new List<string>();
  private readonly IMyScheduler _scheduler;

  public EmailController(IMyScheduler scheduler) {
    _scheduler = scheduler;
  }

  [HttpPost("start")]
  public async Task<IActionResult> StartJob([FromBody] JobRequest jobRequest) {
    try {
      var response = await _scheduler.StartJob(jobRequest);
      _runningJobs.Add(jobRequest.Email);
      return Ok(response);
    } catch (Exception e) {
      return BadRequest(new { Erro = e.Message });
    }
  }

  [HttpPost("stop/{jobName}")]
  public async Task<IActionResult> StopJob(string jobName) {
    try {
      var response = await _scheduler.StopJob(jobName);
      _runningJobs.Remove(jobName);
      return Ok(response);
    } catch (Exception e) {
      return BadRequest(new { Erro = e.Message });
    }
  }

  [HttpPost("running")]
  public IActionResult ListRunningJobs() {
    return Ok(_runningJobs); //obter os jobs da lista criada (que precisa forçar o stop e star a dar manutenção)
  }

  [HttpPost("running2")]
  public async Task<IActionResult> ListRunningJobs2() {
    return Ok(await _scheduler.GetRunningJobs()); //obter os jobs do proprio scheduler
  }
}
