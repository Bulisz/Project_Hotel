using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IPostService
    {
        Task ConfirmPostAsync(int id);
        Task<PostDetailsDTO> CreatePostAsync(PostCreateDTO postDto);
        Task DeletePostAsync(int id);
        Task<IEnumerable<PostDetailsDTO>> GetConfirmedPostsAsync();
        Task<IEnumerable<PostDetailsDTO>> GetNonConfirmedPostsAsync();
    }
}