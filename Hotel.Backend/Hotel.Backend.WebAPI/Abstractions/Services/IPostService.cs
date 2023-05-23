using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IPostService
    {
        Task<PostDetailsDTO> CreatePostAsync(PostCreateDTO postDto);
        Task<IEnumerable<PostDetailsDTO>> GetAllPostsAsync();
    }
}