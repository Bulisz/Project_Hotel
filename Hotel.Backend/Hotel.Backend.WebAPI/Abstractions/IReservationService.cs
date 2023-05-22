using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IReservationService
    {
        Task<ReservationDetailsDTO> CreateReservationAsync(ReservationRequestDTO request);
        Task<List<ReservationDetailsListItemDTO>> GetAllReservationsAsync();
        Task<List<ReservationDetailsListItemDTO>> GetMyOwnReservationsAsync(string userId);
    }
}