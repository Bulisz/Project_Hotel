using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost(nameof(CreatePost))]
    public async Task<ActionResult<PostDetailsDTO>> CreatePost(PostCreateDTO post)
    {
        PostDetailsDTO createdPost = await _postService.CreatePostAsync(post);
        return Ok(createdPost);
    }

    [HttpGet(nameof(GetAllPosts))]
    public async Task<ActionResult<IEnumerable<PostDetailsDTO>>> GetAllPosts()
    {
        IEnumerable<PostDetailsDTO> allPosts = await _postService.GetAllPostsAsync();
        return Ok(allPosts);
    }
}
