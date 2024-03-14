using ExemploBasico;
using Quartz;
using Quartz.Impl;

namespace ExemploBasico.src.Testes {
    public static class First {
        public async static void Begin() {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();

            // Iniciando o scheduler
            await scheduler.Start();

            // Definindo a tarefa
            var job = JobBuilder.Create<HelloJob>()
                .WithIdentity("printJob", "group1")
                .Build();

            // Definindo o trigger
            var trigger = TriggerBuilder.Create()
                .WithIdentity("printTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(1)
                    .RepeatForever())
                .Build();

            // Agendando a tarefa com o trigger
            await scheduler.ScheduleJob(job, trigger);

            // Aguardando para manter o jog rodando
            await Task.Delay(TimeSpan.FromSeconds(60));

            // Parando o scheduler
            await scheduler.Shutdown();
        }
    }
}