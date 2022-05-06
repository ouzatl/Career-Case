using Career.API;
using Career.Common.Configuration;
using Career.Data;
using Career.Data.Mapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

#region Services

var services = builder.Services;
// Add services to the container.

//Swagger
AddSwagger(services);
//PostgreSql Connection
PostgresSqlConnection(services);
//Redis Connection
RedisConnection(services);
//Queue Connection
QueueConnection(services);
//Dependency Injection
DependencyInjection(services);
//AutoMapper
services.AddAutoMapper(typeof(Mapping));


#region Methods

void QueueConnection(IServiceCollection services)
{
    var appSettingsSection = builder.Configuration.GetSection("QueueSettings");
    var appSettings = appSettingsSection.Get<QueueSettings>();

    // services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    // {
    //     cfg.Host(appSettings.HostName, appSettings.VirtualHost,
    //     h =>
    //     {
    //         h.Username(appSettings.UserName);
    //         h.Password(appSettings.UserNameappSettings.UserName);
    //     });

    //     cfg.ExchangeType = ExchangeType.Direct;
    // }));


    services.AddMassTransit(mt =>
                {
                    mt.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(appSettings.HostName, 5672, appSettings.VirtualHost, host =>
                        {
                            host.Username(appSettings.UserName);
                            host.Password(appSettings.Password);
                        });
                    });
                });
    services.AddMassTransitHostedService();
}

void PostgresSqlConnection(IServiceCollection services)
{
    services.AddEntityFrameworkNpgsql().AddDbContext<CareerContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQLConnectionString")));
}

void RedisConnection(IServiceCollection services)
{
    services.AddStackExchangeRedisCache(options =>
     {
         options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
     });
}

void AddSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(x =>
    {
        x.SwaggerDoc("v1", new OpenApiInfo { Title = "Core API", Description = "Career API" });
        x.EnableAnnotations();
    });
}

void DependencyInjection(IServiceCollection services)
{
    var dependency = new DependencyRegister(services);
    dependency.Register();
}

#endregion

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

#endregion

#region App

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Career API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion
