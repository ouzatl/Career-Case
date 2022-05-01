namespace Career.Service.ElasticSearch
{
    public interface IElasticSearchConfiguration
    {
        string Url { get; }
        string UserName { get; }
        string Password { get; }
    }
}