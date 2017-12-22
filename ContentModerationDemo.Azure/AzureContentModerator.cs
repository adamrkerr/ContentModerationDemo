using ContentModerationDemo.Abstraction;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ContentModerationDemo.Azure
{
    public class AzureContentModerator : IContentModerator
    {
        public string ApiKey { get; set; }
        public async Task<ModerationResponse> AnalyzeImage(byte[] imageBytes)
        {
            var client = new RestClient("https://eastus2.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0");
            client.AddDefaultHeader("Ocp-Apim-Subscription-Key", ApiKey);

            var request = new RestRequest("ProcessImage/Evaluate", Method.POST);
            request.AddParameter("image/jpeg", imageBytes, ParameterType.RequestBody);

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
    }
}
