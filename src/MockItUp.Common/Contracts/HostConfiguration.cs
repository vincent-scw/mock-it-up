using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Common.Contracts
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
        public IDictionary<string, int> Hosts { get; set; }
        /// <summary>
        /// The directory for spec files
        /// </summary>
        public string SpecDirectory { get; set; }
    }
}
