using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using ContentModerationDemo.Abstraction;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContentModerationDemo.AWS
{
    public class AWSContentModerator : IAWSContentModerator
    {
        private RegionEndpoint Endpoint { get; set; }

        public AWSContentModerator(string endpointName)
        {
            Endpoint = RegionEndpoint.GetBySystemName(endpointName);
        }

        public async Task<ModerationResponse> AnalyzeImage(MemoryStream imageStream)
        {            
            using (var client  = new AmazonRekognitionClient(Endpoint))
            {
                var request = new DetectModerationLabelsRequest()
                {
                    Image = new Image()
                    {
                        Bytes = imageStream
                    },
                    MinConfidence = 0 //do this so that scores are always returned?
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
                    if (awsResponse.ModerationLabels.Any( s => s.Confidence >= 50))
                    {
                        response.Pass = false;                        
                    }
                    else
                    {
                        response.Pass = true;
                    }

                    response.ModerationScores = awsResponse.ModerationLabels
                            .Select(m => new ModerationScore()
                            {
                                Category = $"{m.ParentName}:{m.Name}",
                                Score = m.Confidence
                            });
                }

                return response;
            }
        }
    }
}
