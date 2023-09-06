using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record UserUpdateDTO
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[A-Z](?!.*  )[a-zA-ZÀ-ÖØ-öø-ÿ ]{1,49}$", ErrorMessage = "2 és 50 betűből álló tulajdonnév")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[A-Z](?!.*  )[a-zA-ZÀ-ÖØ-öø-ÿ ]{1,49}$", ErrorMessage = "2 és 50 betűből álló tulajdonnév")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[a-z0-9.]{2,}[@][a-z0-9]{2,}[.][a-z]{2,}$", ErrorMessage = "Valós email-címet adjon meg")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(?!.*(?:admin|Admin|operator|Operator))[a-zA-Z0-9]{2,30}$", ErrorMessage = "2 és 30 ékezet nélküli betűből és számból állhat")]
    public string Username { get; set; } = string.Empty;
}
