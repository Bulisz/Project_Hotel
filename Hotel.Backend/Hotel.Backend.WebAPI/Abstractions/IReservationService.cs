using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IReservationService
    {
        Task<PostDTO> CreatePostAsync(PostDTO post);
        Task<ReservationDetailsDTO> CreateReservationAsync(ReservationRequestDTO request);
    }
}