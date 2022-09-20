using Amazon.DynamoDBv2.DataModel;
using PostService.Models;
using Microsoft.AspNetCore.Mvc;
using Amazon.Runtime;
using Amazon.SQS.Model;
using Amazon.SQS;
using System.Text.Json;
using Amazon;
using Microsoft.Extensions.Configuration;


namespace DynamoStudentManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private IConfiguration _configuration;
    private readonly IDynamoDBContext _context;

    public PostsController(IDynamoDBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
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
        postRequest.Id = Guid.NewGuid();
        var post = await _context.LoadAsync<Post>(postRequest.Id);
        if (post != null) return BadRequest($"Post with Id {postRequest.Id} Already Exists");
        await _context.SaveAsync(postRequest);
        await SQSPost(postRequest);
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

    [HttpPost("PostSQSQueue", Name = "PostSQSQueue")]
    public async Task SQSPost(Post postRequest)
    {
        var appconfig = _configuration.GetSection("AppConfig").Get<AppConfig>();
        var credentials = new BasicAWSCredentials(appconfig.AccessKeyId, appconfig.SecretAccessKey);
        var client = new AmazonSQSClient(credentials, RegionEndpoint.EUCentral1);

        var request = new SendMessageRequest()
        {
            QueueUrl = appconfig.QueueUrl,
            MessageBody = JsonSerializer.Serialize(postRequest)
        };
        _ = await client.SendMessageAsync(request);
    }
}