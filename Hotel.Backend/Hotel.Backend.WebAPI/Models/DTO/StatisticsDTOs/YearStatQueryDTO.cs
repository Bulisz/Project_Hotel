namespace Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs
{
    public class YearStatQueryDTO
    {
        public int Year { get; set; }
        public List<int>? ChoosedRooms { get; set; } = new List<int>();
    }
}
