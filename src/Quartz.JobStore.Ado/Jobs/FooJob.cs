namespace Quartz.JobStore.Ado.Jobs;

public class FooJob : IJob
{
    public FooJob() // Support Dependecy Injection
    {
    }

    public Task Execute(IJobExecutionContext context)
    {
        var text = context.Trigger.JobDataMap.GetString("text");
        Console.WriteLine($"FooJob: {text}");
        return Task.CompletedTask;
    }
}