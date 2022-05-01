using Career.Contract.Contracts.Company;

namespace Career.Contract.Contracts.Job
{
    public class JobContract
    {
        public int CompanyId { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string Benefits { get; set; }
        public string TypeOfWork { get; set; }
        public double Salary { get; set; }
    }
}