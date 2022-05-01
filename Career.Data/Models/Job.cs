using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Career.Data.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public string Position { get; set; }
        public string TypeOfWork { get; set; }
        public string Description { get; set; }
        public DateTime AvailableUntil { get; set; }
        public double Quailty { get; set; }
        public string Benefits { get; set; }
        public double Salary { get; set; }
        public bool IsActive { get; set; }
        public Company? Company { get; set; }
    }
}