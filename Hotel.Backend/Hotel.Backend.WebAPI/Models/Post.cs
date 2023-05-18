using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Backend.WebAPI.Models;

[Table("Posts")]
public class Post
{
    public int Id { get; set; }
    public  string Text { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Role Role { get; set; }
}
