using Quartz;
using Serilog;

namespace WebApiCustomJobs.src.Services;

public class EmailSender : IJob {
  public Task Execute(IJobExecutionContext context) {
    //! Obtém o email a partir dos dados do trabalho (JobDataMap) que foram passados quando o job foi agendado.
    var email = context.MergedJobDataMap.GetString("Email");
    //! Obtém o intervalo a partir dos dados do trabalho (JobDataMap) que foram passados quando o job foi agendado.
    var intervalo = context.MergedJobDataMap.GetInt("Interval");

    //! Registra uma mensagem de log indicando que o email foi "enviado" e especificando o intervalo em segundos.
    Log.Error($"Email enviado para {email}. Sendo enviado a cada {intervalo} segundos.");

    //! Retorna uma tarefa completada, indicando que o trabalho foi executado com sucesso.
    return Task.CompletedTask;
  }
}
