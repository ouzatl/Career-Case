namespace Career.Contract.ElasticSearchModel
{
    public interface IElasticEntity<TEntityKey>
    {
        TEntityKey Id { get; set; }
    }
}