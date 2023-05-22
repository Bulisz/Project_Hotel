using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories
{
    public interface IPostRepository
    {
        Task<Post> CreatePostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync();
    }
}