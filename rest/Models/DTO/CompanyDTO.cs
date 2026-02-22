using System.ComponentModel.DataAnnotations;

namespace Booking_API.Models.DTO
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public string? Details { get; set; }

        public double Rate { get; set; }

    }
}
