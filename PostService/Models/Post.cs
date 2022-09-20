using Amazon.DynamoDBv2.DataModel;

namespace PostService.Models
{
    [DynamoDBTable("posts")]
    public class Post
    {
        [DynamoDBHashKey("id")]
        public Guid Id { get; set; }

        [DynamoDBProperty("text")]
        public string? Text { get; set; }

        [DynamoDBProperty("userId")]
        public int? UserId { get; set; }

        [DynamoDBProperty("likes")]
        public int? Likes { get; set; }
    }
}