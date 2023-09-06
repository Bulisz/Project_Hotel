using Hotel.Backend.WebAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO
{
    public record RoomSelectorDTO
    {
        [Range(1, 20)]
        public int NumberOfBeds { get; set; }
        [Range(1, 10)]
        public int MaxNumberOfDogs { get; set; }

        [NotInThePast(ErrorMessage = "Csak a jövőbeni dátumokra lehetséges foglalni")]
        public DateTime BookingFrom { get; set; }

        [NotInThePast(ErrorMessage = "Csak a jövőbeni dátumokra lehetséges foglalni")]
        public DateTime BookingTo { get; set; }
        public List<int>? ChoosedEquipments { get; set; } = new List<int>();
    }
}
