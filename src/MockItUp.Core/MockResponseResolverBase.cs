using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UriTemplate.Core;

namespace MockItUp.Core
{
    public abstract class MockResponseResolverBase : IMockResponseResolver
    {
        

        public async Task Resolve(HttpListenerResponse resp, ResponseModel responseModel)
        {

        }
    }
}
