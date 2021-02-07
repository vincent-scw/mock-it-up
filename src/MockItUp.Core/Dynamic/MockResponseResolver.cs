using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UriTemplate.Core;

namespace MockItUp.Core.Dynamic
{
    public class MockResponseResolver : IMockResponseResolver
    {
        public MockResponseResolver()
        {

        }

        public async Task ResolveAsync(HttpListenerContext context, StubItem stub, UriTemplateMatch match)
        {
            
        }
    }
}
