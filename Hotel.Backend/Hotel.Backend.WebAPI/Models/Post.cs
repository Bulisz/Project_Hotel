using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Backend.WebAPI.Models;

[Table("Posts")]
public class Post
{
    public int Id { get; set; }
    public  string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool Confirmed { get; set; }
    public Role Role { get; set; }
}
