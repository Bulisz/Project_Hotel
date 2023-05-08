namespace Hotel.Backend.WebAPI.Models.DTO
{
    public class RoomListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int NumberOfBeds { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Available { get; set; }
        public ICollection<string> EquipmentNames { get; set; } = new List<string>();
        public ICollection<string> ImageURLs { get; set; } = new List<string>();
       
    }
}
