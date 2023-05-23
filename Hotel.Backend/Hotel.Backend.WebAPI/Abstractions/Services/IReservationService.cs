using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface IReservationService
{
    Task<ReservationDetailsDTO> CreateReservationAsync(ReservationRequestDTO request);
    Task DeleteReservationAsync(int reservationId);
    Task<List<ReservationListItemDTO>> GetAllReservationsAsync();
    Task<List<ReservationListItemDTO>> GetMyOwnReservationsAsync(string userId);
}