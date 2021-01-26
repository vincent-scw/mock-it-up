using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Common.Contracts
{
    public interface IMockProvider
    {
        MockTypeEnum MockType { get; }
    }
}
