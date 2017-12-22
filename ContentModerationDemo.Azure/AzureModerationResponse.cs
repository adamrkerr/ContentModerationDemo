using System;
using System.Collections.Generic;
using System.Text;

namespace ContentModerationDemo.Azure
{
    /*
     {"AdultClassificationScore":0.98851776123046875,"IsImageAdultClassified":true,
     "RacyClassificationScore":0.99980628490448,"IsImageRacyClassified":true,
     "AdvancedInfo":[],"Result":true,"Status":{"Code":3000,"Description":"OK","Exception":null},"TrackingId":"EU2_ibiza_8a2dec8b-379f-42b0-a78f-c8b25a8f5b54_ContentModerator.F0_574d13e1-2519-421f-9442-c1f5133038f8"}*/
    class AzureModerationResponse
    {
        public float AdultClassificationScore { get; set; }

        public bool IsImageAdultClassified { get; set; }

        public float RacyClassificationScore { get; set; }

        public bool IsImageRacyClassified { get; set; }
    }
}
