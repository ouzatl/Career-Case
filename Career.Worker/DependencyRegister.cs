using Career.Common.Logging;
using Career.Data.ElasticSearch;
using Career.Data.Repositories.BannedWordsRepository;
using Career.Data.Repositories.CompanyRepository;
using Career.Data.Repositories.JobRepository;
using Career.Service.Services.CompanyService;
using Career.Service.Services.JobService;

namespace Career.Worker
{
    public class DependencyRegister
    {
        private readonly IServiceCollection _services;
        public DependencyRegister(IServiceCollection services)
        {
            _services = services;
        }


        public void Register()
        {
            //Services
            _services.AddScoped<ICompanyService, CompanyService>();
            _services.AddScoped<IJobService, JobService>();

            //Repositories
            _services.AddScoped<ICompanyRepository, CompanyRepository>();
            _services.AddScoped<IJobRepository, JobRepository>();
            _services.AddScoped<IBannedWordsRepository, BannedWordsRepository>();

            //Others
            // _services.AddScoped<AnyHelper>();
            _services.AddScoped<ICompositeLogger, CompositeLogger>();
            // _services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            // _services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            // _services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            _services.AddSingleton<IElasticSearchConfiguration, ElasticSearchConfiguration>();
            _services.AddScoped<IElasticSearchContext, ElasticSearchContext>();
        }
    }
}