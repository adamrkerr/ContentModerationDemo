using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ContentModerationDemo.AWS.Test
{
    public class AWSContentModeratorTest
    {
        [Fact]
        public async Task TestImageStream()
        {
            var moderator = new AWSContentModerator();

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

            var result = await moderator.AnalyzeImage(byteArray, Path.GetFileName(imagePath));

            Assert.False(result.Pass);
            Assert.NotEmpty(result.ModerationScores);
        }
    }
}
