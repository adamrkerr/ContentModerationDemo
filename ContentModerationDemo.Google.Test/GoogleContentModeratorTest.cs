using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ContentModerationDemo.Google.Test
{
    public class GoogleContentModeratorTest
    {
        [Fact]
        public async Task TestImageStream()
        {

            //Set a path to an image that will get results
            var imagePath = @"C:\Users\Adam\Pictures\content moderation\zlonp3btp0d37l4ls3dy.jpg";

            byte[] byteArray;
            using (var stream = new FileStream(imagePath, FileMode.Open))
            {
                var byteList = new List<byte>();
                for (int i = 0; i < stream.Length; i++)
                {
                    byteList.Add((byte)stream.ReadByte());
                }

                byteArray = byteList.ToArray();
            }
            
            var moderator = new GoogleContentModerator("google-service-account.json"); //need to provide this file
            
            var result = await moderator.AnalyzeImage(byteArray);

            Assert.False(result.Pass);
            Assert.NotEmpty(result.ModerationScores);
        }
    }
}
