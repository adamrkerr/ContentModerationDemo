using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ContentModerationDemo.Azure.Test
{
    public class AzureContentModeratorTest
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

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.keys.json", optional: false); //TODO: make your own keys file

            var config = builder.Build();            

            var moderator = new AzureContentModerator(config.GetSection("AzureRegion").Value, config.GetSection("Key").Value);

            var result = await moderator.AnalyzeImage(byteArray, Path.GetFileName(imagePath));

            Assert.False(result.Pass);
            Assert.NotEmpty(result.ModerationScores);
        }
    }
}
