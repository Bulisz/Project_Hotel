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

    [HttpGet(nameof(GetConfirmedPosts))]
    public async Task<ActionResult<IEnumerable<PostDetailsDTO>>> GetConfirmedPosts()
    {
        IEnumerable<PostDetailsDTO> confirmedPosts = await _postService.GetConfirmedPostsAsync();
        return Ok(confirmedPosts);
    }

    [HttpGet(nameof(GetNonConfirmedPosts))]
    public async Task<ActionResult<IEnumerable<PostDetailsDTO>>> GetNonConfirmedPosts()
    {
        IEnumerable<PostDetailsDTO> nonConfirmedPosts = await _postService.GetNonConfirmedPostsAsync();
        return Ok(nonConfirmedPosts);
    }

    [HttpPut(nameof(ConfirmPost))]
    public async Task<IActionResult> ConfirmPost(ConfirmPostDTO confirmPostDTO)
    {
        await _postService.ConfirmPostAsync(confirmPostDTO.Id);
        return Ok();
    }

    [HttpDelete("DeletePost/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        await _postService.DeletePostAsync(id);
        return Ok();
    }
}
