using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controller.Sample
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Sample")]
    public class SampleApiController : ControllerBase
    {
        public SampleApiController() { }

        [HttpGet("[action]")]
        public async Task<List<string>> GetDataSampleAsync()
        {
            return new List<string>() { "Thawitchai", "John", "Bang" };
        }
    }
}