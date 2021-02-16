using System;
using System.Collections.Generic;
using System.Text;

namespace sermon.Core.Entities
{
    public class ServiceRecord
    {
        public ServiceRecord(ServiceInfo serviceInfo)
        {
            ServiceInfo = serviceInfo;
            Timestamp = null;
        }

        public ServiceRecord(ServiceInfo serviceInfo, DateTime timestamp)
        {
            ServiceInfo = serviceInfo;
            Timestamp = timestamp;
        }

        public ServiceInfo ServiceInfo { get; }
        public DateTime? Timestamp { get; }
    }
}
