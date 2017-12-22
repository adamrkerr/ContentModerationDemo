using ContentModerationDemo.Abstraction;
using System;
using System.Threading.Tasks;

namespace ContentModerationDemo.Azure
{
    public class AzureContentModerator : IContentModerator
    {
        public Task<ModerationResponse> AnalyzeImage(byte[] imageBytes)
        {
            throw new NotImplementedException();
        }
    }
}
