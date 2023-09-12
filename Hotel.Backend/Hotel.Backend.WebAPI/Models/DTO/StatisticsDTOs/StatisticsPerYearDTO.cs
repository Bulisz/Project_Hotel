namespace Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs;

public record StatisticsPerYearDTO
{
    public string Name { get; set; } = string.Empty;
    public List<double> Data { get; set; } = new List<double>();
    public string Color { get; set; } = string.Empty;
}
