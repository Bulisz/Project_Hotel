namespace Hotel.Backend.WebAPI.Models.DTO
{
    public record ReservationDetailsListItemDTO
    {
        public int Id { get; set; }
        public DateTime BookingFrom { get; set; }
        public DateTime BookingTo { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
