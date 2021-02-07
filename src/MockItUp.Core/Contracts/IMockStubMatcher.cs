using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UriTemplate.Core;

namespace MockItUp.Core.Contracts
{
    interface IMockStubMatcher
    {
        UriTemplateMatch Match(HttpListenerRequest request, string service, out RuleItem stub);
    }
}
