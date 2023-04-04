using Microsoft.Extensions.Hosting;

namespace Quartz.JobStore.InMemory;

public class App : BackgroundService
{
    readonly ISchedulerFactory _schedulerFactory;
    public App(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var trigger = TriggerBuilder.Create()
            .ForJob("foo_job")
            .WithIdentity("some_trigger_key")
            .UsingJobData("text", "Hello World!")
            .StartAt(DateTime.UtcNow.AddSeconds(5))
            .WithSimpleSchedule(s => s
                .WithIntervalInSeconds(2)
                .WithRepeatCount(2)
            )
            .Build();

        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(trigger);
    }
}