using System;
using Hangfire;
using Hangfire.SqlServer;

namespace HangfireConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("Data Source=(LocalDb)\\MSSQLLocalDB; Database=HangfireTest; Integrated Security=True;", new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                });

            BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

            RecurringJob.AddOrUpdate(
                () => Console.WriteLine("Recurring job!"),
                Cron.Minutely);

            using (var server = new BackgroundJobServer())
            {
                Console.ReadLine();
            }
        }
    }
}
