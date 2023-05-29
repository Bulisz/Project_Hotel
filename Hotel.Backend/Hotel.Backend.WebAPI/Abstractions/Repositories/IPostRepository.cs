using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories
{
    public interface IPostRepository
    {
        Task ConfirmPostAsync(int id);
        Task<Post> CreatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task<IEnumerable<Post>> GetConfirmedPostsAsync();
        Task<IEnumerable<Post>> GetNonConfirmedPostsAsync();
    }
}