using Amazon.DynamoDBv2.DataModel;

namespace PostService.Models
{
    [DynamoDBTable("posts")]
    public class Post
    {
        [DynamoDBHashKey("id")]
        public int? Id { get; set; }

        [DynamoDBProperty("text")]
        public string? Text { get; set; }

        [DynamoDBProperty("userId")]
        public string? UserId { get; set; }

        [DynamoDBProperty("likes")]
        public string? Likes { get; set; }
    }
}