using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record ResetPasswordDTO
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(?=.*\\d)(?!.*\\s).{6,250}$", ErrorMessage = "Legalább 6, legfeljebb 250 karakter hosszúnak kell lennie és tartalmazzon egy számot")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(?=.*\\d)(?!.*\\s).{6,250}$", ErrorMessage = "Legalább 6, legfeljebb 250 karakter hosszúnak kell lennie és tartalmazzon egy számot")]
    [Compare("NewPassword", ErrorMessage = "Azonos jelszót kell megadni.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;
}
