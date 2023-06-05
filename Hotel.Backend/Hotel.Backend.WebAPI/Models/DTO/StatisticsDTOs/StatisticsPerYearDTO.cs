namespace Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs
{
    public class StatisticsPerYearDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public List<double> Percentage { get; set; } = new List<double>();
        public string Color { get; set; } = string.Empty;
    }
}
