namespace Hotel.Backend.WebAPI.Models.DTO;

public class UserLoginDTO
{
    public ApplicationUser? User { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
