using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.JobStore.Ado;
using Quartz.JobStore.Ado.Jobs;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        // services.AddLogging(c => c.ClearProviders());
        services.AddQuartz(c =>
                {
                    c.UsePersistentStore(s =>
                    {
                        s.UsePostgres("Host=localhost;Database=quartz;Username=postgres;Password=abcABC123;");
                    });

                    c.SetProperty("quartz.serializer.type", "json");
                    c.SetProperty("quartz.jobStore.performSchemaValidation", "false");

                    c.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency = 10;
                    });

                    c.AddJob<FooJob>(new JobKey("foo_job"), jc =>
                    {
                        jc.StoreDurably();
                    });
                });

        services.AddQuartzHostedService();

        services.AddHostedService<App>();
    })
    .Build();

await host.RunAsync();

Console.ReadLine();