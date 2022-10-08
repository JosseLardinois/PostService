using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel;

namespace PostService.Models
{
    [DynamoDBTable("posts")]
    public class Post
    {
        [DynamoDBProperty("text")]
        public string? Text { get; set; }

        [DynamoDBHashKey("userId")]
        public Guid UserId { get; set; }

        [DynamoDBRangeKey("creationTime")]
        public DateTime CreationTime { get; set; }


    }
}