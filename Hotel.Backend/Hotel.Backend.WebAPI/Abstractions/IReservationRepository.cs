using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IReservationRepository
    {
        Task<Post> CreatePostAsync(Post post);
        Task<Reservation> CreateReservationAsync(Reservation newReservation);
        Task<IEnumerable<Post>> GetAllPostsAsync();
    }
}