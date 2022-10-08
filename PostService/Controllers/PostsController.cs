using Amazon.DynamoDBv2.DataModel;
using PostService.Models;
using Microsoft.AspNetCore.Mvc;
using Amazon.Runtime;
using Amazon.SQS.Model;
using Amazon.SQS;
using System.Text.Json;
using Amazon;
using Microsoft.Extensions.Configuration;
using PostService.Interfaces;
using PostService.Processors;

namespace DynamoStudentManager.Controllers;

[Route("[controller]")]
public class PostsController : Controller
{
    //to get only posts from last 24 hours: https://www.youtube.com/watch?v=BbUmLRaxZG8&t=1s
    private IConfiguration _configuration;
    private IPostsRepository _repository;

    public PostsController(IPostsRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IEnumerable<Post>> GetAllPostsById(Guid postId)
    {
        return await _repository.All(postId);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(PostInputModel model)
    {

        await _repository.Add(model);
        SQSProcessor processor = new SQSProcessor(_configuration);
        await processor.SQSPost(model);
        return Ok(model);
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<ActionResult> Delete(PostInputModel post)
    {
        await _repository.Remove(post);
        return NoContent();
    }
}