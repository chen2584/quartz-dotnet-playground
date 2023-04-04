// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.JobStore.InMemory;
using Quartz.JobStore.InMemory.Jobs;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddLogging(c => c.ClearProviders());
        services.AddQuartz(c =>
                {
                    c.UseInMemoryStore();

                    c.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency = 10;
                    });

                    c.AddJob<FooJob>(new JobKey("foo_job"), jc =>
                    {
                        jc.StoreDurably();
                        // jc.RequestRecovery();
                    });
                });

        services.AddQuartzHostedService();

        services.AddHostedService<App>();
    })
    .Build();

await host.RunAsync();

Console.ReadLine();