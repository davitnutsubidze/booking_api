using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_API.Models.DTO
{
    public class CompanyEmployeesUpdateDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? LastName { get; set; }

        [Required]
        public int CompanyId { get; set; }

    }
}
