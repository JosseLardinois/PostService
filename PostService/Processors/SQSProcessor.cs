using Amazon.Runtime;
using Amazon.SQS.Model;
using Amazon.SQS;
using System.Text.Json;
using PostService.Models;
using Amazon;

namespace PostService.Processors
{
    public class SQSProcessor
    {
        private IConfiguration _configuration;
        public SQSProcessor(IConfiguration configuration)
        {
             _configuration = configuration;    
        }

        public async Task SQSPost(PostInputModel postInputModel)
        {
            var sqsPostQueue = Environment.GetEnvironmentVariable("AWS_POST_SQS_QUEUE");
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var client = new AmazonSQSClient(credentials, RegionEndpoint.EUCentral1);

            var request = new SendMessageRequest()
            {
                QueueUrl = sqsPostQueue,
                MessageBody = JsonSerializer.Serialize(postInputModel)
            };
            _ = await client.SendMessageAsync(request);
        }

    }
}
