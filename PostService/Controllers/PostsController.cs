using Amazon.DynamoDBv2.DataModel;
using PostService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamoStudentManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IDynamoDBContext _context;

    public PostsController(IDynamoDBContext context)
    {
        _context = context;
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetById(int postId)
    {
        var post = await _context.LoadAsync<Post>(postId);
        if (post == null) return NotFound();
        return Ok(post);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var post = await _context.ScanAsync<Post>(default).GetRemainingAsync();
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(Post postRequest)
    {
        var post = await _context.LoadAsync<Post>(postRequest.Id);
        if (post != null) return BadRequest($"Post with Id {postRequest.Id} Already Exists");
        await _context.SaveAsync(postRequest);
        return Ok(postRequest);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var post = await _context.LoadAsync<Post>(postId);
        if (post == null) return NotFound();
        await _context.DeleteAsync(post);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePost(Post postRequest)
    {
        var post = await _context.LoadAsync<Post>(postRequest.Id);
        if (post == null) return NotFound();
        await _context.SaveAsync(postRequest);
        return Ok(postRequest);
    }
}