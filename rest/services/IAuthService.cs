
using Booking_API.Models.DTO;

namespace Booking_API.services
{
    public interface IAuthService
    {

       Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDTO?>  LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);
    }
}
