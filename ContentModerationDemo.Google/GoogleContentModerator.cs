using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContentModerationDemo.Abstraction;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Grpc.Auth;

namespace ContentModerationDemo.Google
{
    public class GoogleContentModerator : IGoogleContentModerator
    {
        private GoogleCredential Credentials { get; set; }

        public GoogleContentModerator(string credentialFilePath)
        {
            Credentials = GoogleCredential.FromFile(credentialFilePath);
        }

        public async Task<ModerationResponse> AnalyzeImage(byte[] imageBytes)
        {

            var channel = new Grpc.Core.Channel(ImageAnnotatorClient.DefaultEndpoint.Host, Credentials.ToChannelCredentials());
            try
            {
                var client = ImageAnnotatorClient.Create(channel);

                var image = Image.FromBytes(imageBytes);

                var response = await client.DetectSafeSearchAsync(image);

                var moderationResponse = new ModerationResponse();

                moderationResponse.ModerationScores = new[] {
                    new ModerationScore() { Category = $"Adult", Score = ConvertLikelyhood(response.Adult) },
                    new ModerationScore() { Category = $"Medical", Score = ConvertLikelyhood(response.Medical) },
                    new ModerationScore() { Category = $"Spoof", Score = ConvertLikelyhood(response.Spoof) },
                    new ModerationScore() { Category = $"Violent", Score = ConvertLikelyhood(response.Violence) }
                };

                moderationResponse.Pass = !moderationResponse.ModerationScores.Any(s => s.Score > .40F);

                return moderationResponse;
            }
            catch(Exception ex)
            {
                return new ModerationResponse()
                {
                    Pass = false,
                    ModerationScores = new[] { new ModerationScore() { Category = $"ServerError:{ex.Message}", Score = 100 } }
                };
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }

        private static float ConvertLikelyhood(Likelihood likelihood)
        {
            switch (likelihood)
            {
                case Likelihood.Unknown:
                    return 0;
                case Likelihood.VeryLikely:
                    return 1;
                case Likelihood.Likely:
                    return .80F;
                case Likelihood.Possible:
                    return .60F;
                case Likelihood.Unlikely:
                    return .40F;
                case Likelihood.VeryUnlikely:
                    return .20F;
                default:
                    return 0;
            }
        }
    }
}
