namespace Career.Contract.ElasticSearchModel
{
    public class ElasticEntity<TEntityKey> : IElasticEntity<TEntityKey>
    {
        public TEntityKey Id { get; set; }
    }
}