using System;
using System.Collections.Generic;

namespace MockItUp.Core.Models
{
    public class RequestModel
    {
        public string Method { get; set; }

        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                
            }
        }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        
        public BodyType BodyType { get; set; } = BodyType.Direct;

        internal string[] Segments { get; private set; }
    }
}
