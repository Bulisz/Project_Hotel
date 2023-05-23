using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Backend.WebAPI.Models;

[Table("Events")]
public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Schedule { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
