using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record CreateUserForm
{
    [Required]
    [MinLength(2)]
    [RegularExpression("^[a-zA-Z0-9]{2,}$", ErrorMessage = "You can only use characters without accent")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^^[a-z0-9.]{2,}[@][a-z0-9]{2,}[.][a-z]{2,}$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string LastName { get; set; } = string.Empty;
}
