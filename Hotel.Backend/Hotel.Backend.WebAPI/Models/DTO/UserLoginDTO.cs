namespace Hotel.Backend.WebAPI.Models.DTO;

public record UserLoginDTO
{
    public ApplicationUser? User { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
