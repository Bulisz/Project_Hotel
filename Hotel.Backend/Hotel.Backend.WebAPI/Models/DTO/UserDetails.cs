using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO
{
    public class UserDetails
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
