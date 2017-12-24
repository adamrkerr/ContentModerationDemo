using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentModerationDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ModerationController : Controller
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Azure(List<IFormFile> files)
        {           
                        
            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length > 0)
                {
                    var buffer = new byte[formFile.Length];

                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return new OkObjectResult("abc");
        }

        [HttpPost("[action]")]
        public IActionResult AWS()
        {
            throw new NotImplementedException();
        }
    }
}