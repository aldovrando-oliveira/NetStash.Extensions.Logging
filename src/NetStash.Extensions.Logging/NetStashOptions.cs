using System.Collections.Generic;

namespace NetStash.Extensions.Logging
{
    public class NetStashOptions
    {
        public string AppName { get; set; }
        public string Host { get; set;  }
        public int Port { get; set; }
        public Dictionary<string, string> ExtraValues { get; set; }
    }
}