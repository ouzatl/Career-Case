using Career.Common.Configuration;
using Career.Common.Constants;
using Microsoft.Extensions.Configuration;

namespace Career.Data.ElasticSearch
{
    public class ElasticSearchConfiguration : IElasticSearchConfiguration
    {
        public IConfiguration _configuration { get; }
        public ElasticSearchSettings _elasticSettings { get; }


        public ElasticSearchConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            var appSettingsSection = configuration.GetSection("ElasticSearchSettings");
            _elasticSettings = appSettingsSection.Get<ElasticSearchSettings>();
        }

        public string Url => _elasticSettings.HostUrl;
        public string UserName => _elasticSettings.UserName;
        public string Password => _elasticSettings.Password;
    }
}