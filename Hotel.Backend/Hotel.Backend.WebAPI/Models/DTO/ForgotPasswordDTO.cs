using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO
{
    public record ForgotPasswordDTO
    {
        [Required]
        [RegularExpression("^[a-z0-9.]{2,}[@][a-z0-9]{2,}[.][a-z]{2,}$", ErrorMessage = "Valós email-címet adjon meg")]
        public string Email { get; set; } = string.Empty;
    }
}
