using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Core.Contracts
{
    public class HostConfiguration
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Host Url
        /// </summary>
        public IDictionary<string, string> Hosts { get; set; }
    }
}
