using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContentModerationDemo.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentModerationDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ModerationController : Controller
    {
        private readonly IAWSContentModerator _awsModerator;
        private readonly IAzureContentModerator _azureModerator;
        public ModerationController(IAWSContentModerator awsModerator, IAzureContentModerator azureModerator)
        {
            _awsModerator = awsModerator;
            _azureModerator = azureModerator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Azure(List<IFormFile> files)
        {

            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var buffer = new byte[formFile.Length];

                        using (var stream = new MemoryStream(buffer))
                        {
                            await formFile.CopyToAsync(stream);
                            
                            var moderationResult = await _azureModerator.AnalyzeImage(buffer, formFile.FileName);

                            return new OkObjectResult(moderationResult);
                        }
                    }
                }

                return new NotFoundResult();
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
            
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AWS()
        {
            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {                        
                        using (var stream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(stream);

                            var moderationResult = await _awsModerator.AnalyzeImage(stream);

                            return new OkObjectResult(moderationResult);
                        }
                    }
                }

                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}