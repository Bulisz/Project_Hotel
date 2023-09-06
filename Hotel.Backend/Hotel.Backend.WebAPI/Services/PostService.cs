using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PostService(IMapper mapper, IPostRepository postRepository, IDateTimeProvider dateTimeProvider)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task ConfirmPostAsync(int id)
    {
        await _postRepository.ConfirmPostAsync(id);
    }

    public async Task<PostDetailsDTO> CreatePostAsync(PostCreateDTO postDto)
    {
        Post postToCreate = _mapper.Map<Post>(postDto);
        postToCreate.CreatedAt = _dateTimeProvider.Now;
        postToCreate.Confirmed = postToCreate.Role == Role.Guest ? false : true;
        var p1 = await _postRepository.CreatePostAsync(postToCreate);
        var p2 = _mapper.Map<PostDetailsDTO>(p1);
        return p2;
    }

    public async Task DeletePostAsync(int id)
    {
        await _postRepository.DeletePostAsync(id);
    }

    public async Task<IEnumerable<PostDetailsDTO>> GetConfirmedPostsAsync()
    {
        return _mapper.Map<IEnumerable<PostDetailsDTO>>(await _postRepository.GetConfirmedPostsAsync());
    }

    public async Task<IEnumerable<PostDetailsDTO>> GetNonConfirmedPostsAsync()
    {
        return _mapper.Map<IEnumerable<PostDetailsDTO>>(await _postRepository.GetNonConfirmedPostsAsync());
    }
}
