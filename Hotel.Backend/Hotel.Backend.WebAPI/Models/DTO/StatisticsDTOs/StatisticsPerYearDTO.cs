namespace Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs
{
    public class StatisticsPerYearDTO
    {
       
        public string Name { get; set; }
        public List<double> Data { get; set; } = new List<double>();
        public string Color { get; set; } = string.Empty;
    }
}
