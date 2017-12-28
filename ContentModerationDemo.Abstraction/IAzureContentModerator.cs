using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentModerationDemo.Abstraction
{
    public interface IAzureContentModerator
    {
        Task<ModerationResponse> AnalyzeImage(byte[] imageBytes, string imageName);
    }
}
