using Career.Common.Configuration;
using Career.Consumer.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Career.Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            DependencyInjection(services);

            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            IConfiguration config = builder.Build();
            var appSettings = config.GetSection("QueueSettings").Get<QueueSettings>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<JobChangedMessageConsumer>();
            });

            var bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(appSettings.HostName, appSettings.VirtualHost, h =>
                    {
                        h.Username(appSettings.UserName);
                        h.Password(appSettings.Password);
                    });

                factory.UseRetry(r => r.Immediate(5));

                factory.ReceiveEndpoint("test-queue", endpoint =>
                {
                    endpoint.Consumer<JobChangedMessageConsumer>();
                });

                // factory.UseCircuitBreaker(configurator =>
                // {
                //     configurator.TrackingPeriod = TimeSpan.FromMinutes(1);
                //     configurator.TripThreshold = 15;
                //     configurator.ActiveThreshold = 10;
                //     configurator.ResetInterval = TimeSpan.FromMinutes(5);
                // });

                // factory.UseMessageRetry(r => r.Immediate(5));

                // factory.UseRateLimit(1000, TimeSpan.FromMinutes(1));
            });

            await bus.StartAsync();

            Console.ReadLine();

            await bus.StopAsync();
        }

        static void DependencyInjection(IServiceCollection services)
        {
            var dependency = new DependencyRegister(services);
            dependency.Register();
        }
    }
}