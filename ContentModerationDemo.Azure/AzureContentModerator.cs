using ContentModerationDemo.Abstraction;
using RestSharp;
using System;
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
            //var client = new RestClient("https://eastus2.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0");

            //var request = new RestRequest("ProcessImage/Evaluate", Method.POST);
            //request.AddFile("value", filePath);
            //request.AddHeader("Content-Type", "image/jpeg");
            //request.AddHeader("Ocp-Apim-Subscription-Key", ApiKey);

            //var response = client.Execute(request);

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey);

            // Request parameters
            queryString["CacheImage"] = "false";
            var uri = "https://eastus2.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessImage/Evaluate?" + queryString;

            HttpResponseMessage response;
            
            using (var content = new ByteArrayContent(imageBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                response = await client.PostAsync(uri, content);

                var responseValue = await response.Content.ReadAsStringAsync();
            }

            throw new NotImplementedException();
        }
    }
}
