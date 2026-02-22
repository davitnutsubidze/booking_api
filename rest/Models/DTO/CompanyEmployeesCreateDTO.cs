using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_API.Models.DTO
{
    public class CompanyEmployeesCreateDTO
    {

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? LastName { get; set; }

        [Required]
        public int CompanyId { get; set; }

    }
}
