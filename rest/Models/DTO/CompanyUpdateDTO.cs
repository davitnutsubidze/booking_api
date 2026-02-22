using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models.DTO
{
    public class CompanyUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public required string Name { get; set; }

        public string? Details { get; set; }

        public double Rate { get; set; }

    }
}
