using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IPostService postService, ILogger<PostsController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    [HttpPost(nameof(CreatePost))]
    public async Task<ActionResult<PostDetailsDTO>> CreatePost(PostCreateDTO post)
    {
        try
        {
            PostDetailsDTO createdPost = await _postService.CreatePostAsync(post);
            return Ok(createdPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet(nameof(GetConfirmedPosts))]
    public async Task<ActionResult<IEnumerable<PostDetailsDTO>>> GetConfirmedPosts()
    {
        try
        {
            IEnumerable<PostDetailsDTO> confirmedPosts = await _postService.GetConfirmedPostsAsync();
            return Ok(confirmedPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet(nameof(GetNonConfirmedPosts))]
    public async Task<ActionResult<IEnumerable<PostDetailsDTO>>> GetNonConfirmedPosts()
    {
        try
        {
            IEnumerable<PostDetailsDTO> nonConfirmedPosts = await _postService.GetNonConfirmedPostsAsync();
            return Ok(nonConfirmedPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut(nameof(ConfirmPost))]
    public async Task<IActionResult> ConfirmPost(ConfirmPostDTO confirmPostDTO)
    {
        try
        {
            await _postService.ConfirmPostAsync(confirmPostDTO.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpDelete("DeletePost/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            await _postService.DeletePostAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
