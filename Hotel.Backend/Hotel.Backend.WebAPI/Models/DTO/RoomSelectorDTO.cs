namespace Hotel.Backend.WebAPI.Models.DTO
{
    public class RoomSelectorDTO
    {
        public int Guest { get; set; }
        public int Dog { get; set; }
        public DateTime BookingFrom { get; set; }
        public DateTime BookingTo { get; set; }
        public List<int>? ChoosedEquipments { get; set; } = new List<int>();

    }
}
