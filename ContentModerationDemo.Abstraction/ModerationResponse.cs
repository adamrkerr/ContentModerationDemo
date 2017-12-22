using System;
using System.Collections.Generic;

namespace ContentModerationDemo.Abstraction
{
    public class ModerationResponse
    {
        public bool Pass { get; set; }
        
        public IEnumerable<ModerationScore> ModerationScores { get; set; }
    }
}
