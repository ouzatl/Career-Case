using Career.Common.Configuration;
using Career.Consumer.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Career.Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);

            IConfiguration config = builder.Build();

            var appSettings = config.GetSection("QueueSettings").Get<QueueSettings>();

            var bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(appSettings.HostName, appSettings.VirtualHost, h =>
                    {
                        h.Username(appSettings.UserName);
                        h.Password(appSettings.Password);
                    });

                factory.ReceiveEndpoint(endpoint =>
                {
                    endpoint.Consumer<JobChangedMessageConsumer>();
                });
                factory.UseCircuitBreaker(configurator =>
                {
                    configurator.TrackingPeriod = TimeSpan.FromMinutes(1);
                    configurator.TripThreshold = 15;
                    configurator.ActiveThreshold = 10;
                    configurator.ResetInterval = TimeSpan.FromMinutes(5);
                });

                // factory.UseMessageRetry(r => r.Immediate(5));

                // factory.UseRateLimit(1000, TimeSpan.FromMinutes(1));
            });

            await bus.StartAsync();

            Console.ReadLine();

            await bus.StopAsync();
        }
    }
}