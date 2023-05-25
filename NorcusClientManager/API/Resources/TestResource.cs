using Grapevine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusClientManager.API.Resources
{
    [RestResource]
    public class TestResource
    {
        [RestRoute("Get", "/api/test")]
        public async Task Test(IHttpContext context)
        {
            await context.Response.SendResponseAsync("Successfully hit the test route!").ConfigureAwait(false);
        }
    }
}
