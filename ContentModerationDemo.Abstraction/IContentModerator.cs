using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentModerationDemo.Abstraction
{
    public interface IContentModerator
    {
        Task<ModerationResponse> AnalyzeImage(byte[] imageBytes)
    }
}
