using Career.API;
using Career.Common.Configuration;
using MassTransit;
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

#region Methods

void QueueConnection(IServiceCollection services)
{
    var appSettingsSection = builder.Configuration.GetSection("appsettings");
    var appSettings = appSettingsSection.Get<Appsettings>();

    services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(appSettings.QueueSettings.HostName, appSettings.QueueSettings.VirtualHost,
        h =>
        {
            h.Username(appSettings.QueueSettings.UserName);
            h.Password(appSettings.QueueSettings.Password);
        });

        cfg.ExchangeType = ExchangeType.Direct;
    }));
}

void PostgresSqlConnection(IServiceCollection services)
{
    // services.AddEntityFrameworkNpgsql().AddDbContext<CareerPostgreSqlContext>(opt =>
    // opt.UseNpgsql(Configuration.GetConnectionString("PostgresSQLConnectionString")));
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
