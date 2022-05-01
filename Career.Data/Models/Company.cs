using System.ComponentModel.DataAnnotations;

namespace Career.Data.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public int MaxPostCount { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}