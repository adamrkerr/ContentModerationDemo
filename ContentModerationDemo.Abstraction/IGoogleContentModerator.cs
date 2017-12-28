using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ContentModerationDemo.Abstraction
{
    public interface IGoogleContentModerator
    {
        Task<ModerationResponse> AnalyzeImage(byte[] imageBytes);
    }
}
