using Career.Contract.ElasticSearchModel;
using Nest;

namespace Career.ElasticSearch.ElasticSearch
{
    public interface IElasticSearchContext
    {
        bool CreateIndex<T, TKey>(string index) where T : ElasticEntity<TKey>;
        bool ExistIndex(string indexName);
        Task<bool> AddData<T, TKey>(string indexName, T data) where T : ElasticEntity<TKey>;
        Task<ISearchResponse<T>> SimpleSearch<T, TKey>(string indexName, SearchDescriptor<T> query) where T : ElasticEntity<TKey>;
    }
}