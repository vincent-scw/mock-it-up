using System.Collections.Generic;

namespace MockItUp.Core.Models
{
    public class HostConfiguration
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; } = "v1";
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; } = "*";
        /// <summary>
        /// Port for control server
        /// </summary>
        public int ControlPort { get; set; } = 30000;
        /// <summary>
        /// Accept connections simultaneously
        /// </summary>
        public int AcceptConnections { get; set; } = 5;
        /// <summary>
        /// Services
        /// </summary>
        public IDictionary<string, int> Services { get; set; } = new Dictionary<string, int>
        {
            { "*", 5000 } // Default service
        };

        /// <summary>
        /// The directory for spec files
        /// </summary>
        public string SpecDirectory { get; set; } = "/etc/mockitup.d/specs/";
        /// <summary>
        /// The directory for payload files
        /// </summary>
        public string PayloadDirectory { get; set; } = "/etc/mockitup.d/payloads/";
    }
}
