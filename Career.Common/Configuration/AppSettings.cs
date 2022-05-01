namespace Career.Common.Configuration
{
    public class Appsettings
    {
        public QueueSettings QueueSettings { get; set; }
        public ElasticSearchSettings ElasticSearchSettings { get; set; }

        public Appsettings()
        {

        }
        public Appsettings(QueueSettings queueSettings)
        {
            QueueSettings = queueSettings;
        }

        public Appsettings(ElasticSearchSettings elasticSearchSettings)
        {
            ElasticSearchSettings = elasticSearchSettings;
        }
    }
}