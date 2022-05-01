using Career.Common.Configuration;
using Career.Common.Constants;
using Microsoft.Extensions.Configuration;

namespace Career.Service.ElasticSearch
{
    public class ElasticSearchConfiguration : IElasticSearchConfiguration
    {
        public IConfiguration _configuration { get; }

        public ElasticSearchConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Url => _configuration.GetConnectionString(ConfigurationConstants.ElasticSearchHostUrl);
        public string UserName => _configuration.GetConnectionString(ConfigurationConstants.ElasticSearchUserName);
        public string Password => _configuration.GetConnectionString(ConfigurationConstants.ElasticSearchPassword);
    }
}