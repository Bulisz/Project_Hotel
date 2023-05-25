using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO
{
    public record UserUpdateDTO
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[a-zA-Z0-9.]{2,}@[a-zA-Z0-9]{2,}.[a-zA-Z0-9]{2,}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z0-9]{2,}$", ErrorMessage = "You can only use characters without accent")]
        public string Username { get; set; } = string.Empty;
    }
}
