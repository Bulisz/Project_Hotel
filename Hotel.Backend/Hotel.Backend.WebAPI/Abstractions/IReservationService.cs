using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IReservationService
    {
        Task<ReservationDetailsDTO> CreateReservationAsync(ReservationRequestDTO request);
        Task<List<ReservationDetailsListItemDTO>> GetAllReservationsAsync();
        Task<List<ReservationDetailsListItemDTO>> GetMyOwnReservationsAsync(string userId);
    }
namespace Hotel.Backend.WebAPI.Abstractions;

public interface IReservationService
{
    Task<PostDetailsDTO> CreatePostAsync(PostCreateDTO post);
    Task<ReservationDetailsDTO> CreateReservationAsync(ReservationRequestDTO request);
    Task<IEnumerable<PostDetailsDTO>> GetAllPostsAsync();
}