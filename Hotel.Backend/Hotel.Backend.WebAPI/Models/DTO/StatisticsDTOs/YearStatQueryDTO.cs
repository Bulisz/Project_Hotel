namespace Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs;

public record YearStatQueryDTO
{
    public int Year { get; set; }
    public List<string>? ChoosedRooms { get; set; } = new List<string>();
}
