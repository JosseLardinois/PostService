using Amazon.DynamoDBv2.DataModel;

namespace PostService.Models
{
    public class PostInputModel
    {
        public string? Text { get; set; }
        public Guid UserId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
