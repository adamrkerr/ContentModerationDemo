using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ContentModerationDemo.AWS
{
    public class AWSContentModerator
    {
        public async Task<object> AnalyzeImage(byte[] imageBytes)
        {
            var endpoint = Amazon.RegionEndpoint.USWest2;
            using (var client  = new AmazonRekognitionClient(endpoint))
            {
                var request = new DetectModerationLabelsRequest()
                {
                    Image = new Image()
                    {
                        Bytes = new MemoryStream(imageBytes)
                    }
                };

                var responde = await client.DetectModerationLabelsAsync(request);

                return responde;
            }
        }
    }
}
