using Career.Contract.ElasticSearchModel;
using Nest;

namespace Career.Service.ElasticSearch
{
    public interface IElasticSearchService
    {
        bool CreateIndex<T, TKey>(string index) where T : ElasticEntity<TKey>;
        bool ExistIndex(string indexName);
        bool AddData<T, TKey>(string indexName, T data) where T : ElasticEntity<TKey>;
        ISearchResponse<T> SimpleSearch<T, TKey>(string indexName, SearchDescriptor<T> query) where T : ElasticEntity<TKey>;
    }
}