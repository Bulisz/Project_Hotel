namespace Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs
{
    public class YearStatQueryDTO
    {
        public int Year { get; set; }
        public List<string>? ChoosedRooms { get; set; } = new List<string>();
    }
}
