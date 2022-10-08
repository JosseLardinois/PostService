using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using PostService.Interfaces;
using Amazon;
using PostService.Models;

namespace PostService.Repositories
{
    public class PostsRepository : IPostsRepository
    {

        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public PostsRepository()
        {
            _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
            _context = new DynamoDBContext(_client);
        }
        public async Task Add(PostInputModel entity)
        {
            var post = new Post
            {
                UserId = entity.UserId,
                Text = entity.Text,
                CreationTime = DateTime.Now,

            };

            await _context.SaveAsync(post);
        }

        public async Task<IEnumerable<Post>> All(Guid postId)
        {
            return await _context.QueryAsync<Post>(postId).GetRemainingAsync();
        }

        public async Task Remove(PostInputModel entity)
        {
            var post = new Post
            {
                UserId=entity.UserId,
                CreationTime = entity.CreationTime,
                Text = entity.Text
            };
            var _follower = await _context.LoadAsync<Post>(post);
            await _context.DeleteAsync(_follower);
        }

        public async Task<Post> Single(Guid postId)
        {
            return await _context.LoadAsync<Post>(postId); //.QueryAsync<Follower>(FollowerId.ToString()).GetRemainingAsync();
        }



    }
}
