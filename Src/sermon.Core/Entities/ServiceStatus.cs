using System;
using System.Collections.Generic;
using System.Text;

namespace sermon.Core.Entities
{
    public class ServiceStatus
    {
        public ServiceStatus(ServiceRecord serviceRecord, bool alive)
        {
            ServiceRecord = serviceRecord;
            Alive = alive;
        }

        public ServiceRecord ServiceRecord { get; }
        public bool Alive { get; }
    }
}
