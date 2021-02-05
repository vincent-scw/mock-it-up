using System.Collections.Generic;

namespace MockItUp.Core.Models
{
    public class HostConfiguration
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; } = "*";
        /// <summary>
        /// Accept connections simultaneously
        /// </summary>
        public int AcceptConnections { get; set; } = 5;
        /// <summary>
        /// Services
        /// </summary>
        public IDictionary<string, int> Services { get; set; }
        /// <summary>
        /// The directory for spec files
        /// </summary>
        public string SpecDirectory { get; set; }
        /// <summary>
        /// The directory for payload files
        /// </summary>
        public string PayloadDirectory { get; set; }
    }
}
