using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using ContentModerationDemo.Abstraction;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContentModerationDemo.AWS
{
    public class AWSContentModerator : IContentModerator
    {
        public async Task<ModerationResponse> AnalyzeImage(byte[] imageBytes, string imageName)
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

                var awsResponse = await client.DetectModerationLabelsAsync(request);

                var response = new ModerationResponse();
                
                if(awsResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.Pass = false;
                    response.ModerationScores = new[] { new ModerationScore() { Category=$"ServerError:{awsResponse.HttpStatusCode}", Score=100 } };
                }
                else
                {
                    if (awsResponse.ModerationLabels.Any())
                    {
                        response.Pass = false;

                        response.ModerationScores = awsResponse.ModerationLabels
                            .Select(m => new ModerationScore()
                            {
                                Category = $"{m.ParentName}:{m.Name}",
                                Score = m.Confidence
                            });
                    }
                    else
                    {
                        response.Pass = true;
                    }
                }

                return response;
            }
        }
    }
}
