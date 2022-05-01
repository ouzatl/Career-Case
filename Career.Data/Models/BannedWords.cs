using System.ComponentModel.DataAnnotations;

namespace Career.Data.Models
{
    public class BannedWords
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}