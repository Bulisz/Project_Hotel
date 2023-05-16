using Hotel.Backend.WebAPI.Helpers;

namespace Hotel.Backend.WebAPI.Models.DTO
{
    public record ReservationRequestDTO
    {
        public string UserId { get; set; } = string.Empty;
        public int RoomId { get; set; }

        [NotInThePast(ErrorMessage = "Csak a jövőbeni dátumokra lehetséges foglalni")]
        public DateTime BookingFrom { get; set; }

        [NotInThePast(ErrorMessage = "Csak a jövőbeni dátumokra lehetséges foglalni")]
        public DateTime BookingTo { get; set; }
    }
}