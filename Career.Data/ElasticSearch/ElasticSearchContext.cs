using System.Net;
using Career.Common.Logging;
using Career.Contract.ElasticSearchModel;
using Nest;

namespace Career.Data.ElasticSearch
{
    public class ElasticSearchContext : IElasticSearchContext
    {
        public IElasticClient Client { get; set; }
        private readonly IElasticSearchConfiguration _elasticSearchConfiguration;
        private readonly ICompositeLogger _logger;

        public ElasticSearchContext(
            IElasticSearchConfiguration elasticSearchConfiguration,
            ICompositeLogger logger)
        {
            _elasticSearchConfiguration = elasticSearchConfiguration;
            Client = GetClient();
            _logger = logger;
        }

        private IElasticClient GetClient()
        {
            var connectionString = new ConnectionSettings(new Uri(_elasticSearchConfiguration.Url))
                .DisablePing()
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);

            if (!string.IsNullOrEmpty(_elasticSearchConfiguration.UserName) && !string.IsNullOrEmpty(_elasticSearchConfiguration.Password))
                connectionString.BasicAuthentication(_elasticSearchConfiguration.UserName, _elasticSearchConfiguration.Password);

            return new ElasticClient(connectionString);
        }

        public bool ExistIndex(string indexName)
        {
            return Client.Indices.Exists(indexName).Exists;
        }

        public bool CreateIndex<T, TKey>(string index) where T : ElasticEntity<TKey>
        {
            var indexExist = ExistIndex(index);
            if (indexExist)
                return true;

            var result = Client.Indices.Create(index, x => x.Index(index).Map(a => a.AutoMap()).Settings(o => o.NumberOfShards(3).NumberOfReplicas(1)));

            return result.Acknowledged;
        }

        public async Task<bool> AddData<T, TKey>(string indexName, T data) where T : ElasticEntity<TKey>
        {
            bool result = false;

            try
            {
                if (CreateIndex<T, TKey>(indexName))
                    result = await InsertDocument<T, TKey>(indexName, data);
            }
            catch (System.Exception ex)
            {
                _logger.Error("ElasticSearchService AddData Method", ex);
            }

            return result;
        }

        private async Task<bool> InsertDocument<T, TKey>(string indexName, T data) where T : ElasticEntity<TKey>
        {
            var result = await Client.IndexAsync(data, ss => ss.Index(indexName));
            if (result.ApiCall?.HttpStatusCode == (int)HttpStatusCode.Conflict)
                await Client.UpdateAsync<T>(result.Id, a => a.Index(indexName).Doc(data));

            return true;
        }

        public async Task<ISearchResponse<T>> SimpleSearch<T, TKey>(string indexName, SearchDescriptor<T> query) where T : ElasticEntity<TKey>
        {
            try
            {
                query.Index(indexName);
                var response = await Client.SearchAsync<T>(query);

                return response;
            }
            catch (System.Exception ex)
            {
                _logger.Error("ElasticSearchService SimpleSearch Method", ex);
            }

            return null;
        }
    }
}