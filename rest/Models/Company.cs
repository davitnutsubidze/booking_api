using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public string? Details { get; set; }

        public double Rate { get; set; }

        public DateTime? CreatedDate { get; set; } 

        public DateTime? UpdatedDate { get; set; }

        public ICollection<CompanyEmployees>? Employees { get; set; }
    }
}
