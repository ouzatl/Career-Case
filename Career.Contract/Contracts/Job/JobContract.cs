using Swashbuckle.AspNetCore.Annotations;

namespace Career.Contract.Contracts.Job
{
    public class JobContract
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string Benefits { get; set; }
        public string TypeOfWork { get; set; }
        public double Salary { get; set; }
    }
}