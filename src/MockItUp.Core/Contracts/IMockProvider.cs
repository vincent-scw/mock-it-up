using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Core.Contracts
{
    public interface IMockProvider
    {
        MockTypeEnum MockType { get; }
    }
}
