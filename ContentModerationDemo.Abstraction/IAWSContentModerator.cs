using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ContentModerationDemo.Abstraction
{
    public interface IAWSContentModerator
    {
        Task<ModerationResponse> AnalyzeImage(MemoryStream imageStream);
    }
}
