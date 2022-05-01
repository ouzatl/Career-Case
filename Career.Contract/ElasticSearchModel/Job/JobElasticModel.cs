namespace Career.Contract.ElasticSearchModel.Job
{
    public class JobElasticModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public double Quailty { get; set; }
        public string Benefits { get; set; }
        public string Type { get; set; }
        public double Salary { get; set; }
    }
}