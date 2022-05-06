namespace Career.Contract.ElasticSearchModel.Job
{
    public class JobElasticModel : ElasticEntity<string>
    {
        public int CompanyId { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public double Quailty { get; set; }
        public string Benefits { get; set; }
        public string TypeOfWork { get; set; }
        public double Salary { get; set; }
    }
}