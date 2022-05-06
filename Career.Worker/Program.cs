using Career.Worker;
using Career.Worker.Consumers;
using MassTransit;

var configuration = new ConfigurationBuilder()
       .AddEnvironmentVariables()
       .AddCommandLine(args)
       .AddJsonFile("appsettings.json")
       .Build();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
        {
            builder.Sources.Clear();
            builder.AddConfiguration(configuration);
        })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        DependencyInjection(services);
        services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", 5672, "/", h =>
                                  {
                                      h.Username("guest");
                                      h.Password("guest");
                                  });

                    cfg.ReceiveEndpoint("test-queue", endpoint =>
                    {
                        endpoint.UseMessageRetry(r => r.Immediate(5));
                        endpoint.ConfigureConsumer<JobChangedMessageConsumer>(context);
                    });
                });

                x.AddConsumer<JobChangedMessageConsumer>();
            });
        services.AddMassTransitHostedService();
    })
    .Build();

static void DependencyInjection(IServiceCollection services)
{
    var dependency = new DependencyRegister(services);
    dependency.Register();
}

await host.RunAsync();
