using System;
using System.Collections.Generic;
using System.Text;

namespace sermon.Core.Entities
{
    public class ServiceInfo
    {
        public ServiceInfo(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; }
        public string Url { get; }

        public override string ToString()
        {
            return $"{Name}: {Url}";
        }
    }
}
