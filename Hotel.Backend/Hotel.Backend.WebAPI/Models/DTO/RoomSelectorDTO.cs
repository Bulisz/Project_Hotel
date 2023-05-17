using Hotel.Backend.WebAPI.Helpers;

namespace Hotel.Backend.WebAPI.Models.DTO
{
    public class RoomSelectorDTO
    {
        public int Guest { get; set; }
        public int Dog { get; set; }

        [NotInThePast(ErrorMessage = "Csak a jövőbeni dátumokra lehetséges foglalni")]
        public DateTime BookingFrom { get; set; }

        [NotInThePast(ErrorMessage = "Csak a jövőbeni dátumokra lehetséges foglalni")]
        public DateTime BookingTo { get; set; }
        public List<int>? ChoosedEquipments { get; set; } = new List<int>();

    }
}
