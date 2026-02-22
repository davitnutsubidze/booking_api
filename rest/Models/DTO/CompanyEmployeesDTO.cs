using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_API.Models.DTO
{
    public class CompanyEmployeesDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? LastName { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public string? CompanyName { get; set; }

    }
}
