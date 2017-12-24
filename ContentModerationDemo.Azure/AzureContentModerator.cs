using ContentModerationDemo.Abstraction;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ContentModerationDemo.Azure
{
    public class AzureContentModerator : IContentModerator
    {
        private const string API_URL_FORMAT = "https://{0}.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0";
        private const string API_ENDPOINT = "ProcessImage/Evaluate";
        private const string API_KEY_HEADER = "Ocp-Apim-Subscription-Key";

        private const string MIME_JPEG = "image/jpeg";
        private const string MIME_GIF = "image/gif";
        private const string MIME_PNG = "image/png";
        private const string MIME_BMP = "image/bmp";

        public AzureContentModerator(string azureRegion, string apiKey)
        {
            ApiKey = apiKey;
            AzureRegion = azureRegion;
        }

        public string AzureRegion { get; set; }

        public string ApiKey { get; private set; }

        public async Task<ModerationResponse> AnalyzeImage(byte[] imageBytes, string imageName)
        {
            var client = new RestClient(String.Format(API_URL_FORMAT, AzureRegion));
            client.AddDefaultHeader(API_KEY_HEADER, ApiKey);

            var request = new RestRequest(API_ENDPOINT, Method.POST);
            request.AddParameter(DetermineMimeType(imageName), imageBytes, ParameterType.RequestBody);

            var apiResponse = await client.ExecuteTaskAsync<AzureModerationResponse>(request);

            var response = new ModerationResponse();

            if (apiResponse.ResponseStatus != ResponseStatus.Completed || !apiResponse.IsSuccessful)
            {
                response.Pass = false;
                response.ModerationScores = new[] { new ModerationScore() { Category = $"ServerError:{apiResponse.StatusDescription}", Score = 100 } };
            }
            else
            {
                if (apiResponse.Data.IsImageAdultClassified || apiResponse.Data.IsImageRacyClassified)
                {
                    response.Pass = false;

                    var list = new List<ModerationScore>();

                    if (apiResponse.Data.IsImageAdultClassified)
                    {
                        list.Add(new ModerationScore { Category = "Adult", Score = apiResponse.Data.AdultClassificationScore });
                    }

                    if (apiResponse.Data.IsImageRacyClassified)
                    {
                        list.Add(new ModerationScore { Category = "Racy", Score = apiResponse.Data.RacyClassificationScore});
                    }

                    response.ModerationScores = list;
                }
                else
                {
                    response.Pass = true;
                }
            }

            return response;
        }

        internal static string DetermineMimeType(string imageName)
        {
            switch (Path.GetExtension(imageName))
            {
                case "jpg":
                case "jpeg":
                    return MIME_JPEG;
                case "png":
                    return MIME_PNG;
                case "bmp":
                    return MIME_BMP;
                case "gif":
                    return MIME_GIF;
                default:
                    return "unknown";
            }
        }
    }
}
