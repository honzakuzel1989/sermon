using Microsoft.Extensions.Caching.Memory;
using sermon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace sermon.Core.Services
{
    public class ServicesStatusProvider : IServicesStatusProvider
    {
        private const int DEFAULT_FAIL_INTERVAL_S = 30;

        private readonly IServicesInfoProvider _servicesInfoProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly int _failInterval;

        public ServicesStatusProvider(
            IServicesInfoProvider servicesInfoProvider,
            IMemoryCache memoryCache)
        {
            _servicesInfoProvider = servicesInfoProvider;
            _memoryCache = memoryCache;

            _failInterval = int.TryParse(Environment.GetEnvironmentVariable("SERMON_FAIL_INTERVAL_S"), out var f)
                ? f
                : DEFAULT_FAIL_INTERVAL_S;
        }

        public ServiceStatus[] Get()
        {
            List<ServiceStatus> serviceStatuses = new List<ServiceStatus>();
            foreach (var serviceInfo in _servicesInfoProvider.Get())
            {
                serviceStatuses.Add(_memoryCache.TryGetValue(serviceInfo.Name, out ServiceRecord serviceRecord)
                    ? new ServiceStatus(serviceRecord, IsAlive(serviceRecord))
                    : new ServiceStatus(new ServiceRecord(serviceInfo), false));
            }
            return serviceStatuses.ToArray();
        }

        private bool IsAlive(ServiceRecord record)
        {
            return record.Timestamp > DateTime.Now.AddSeconds(-_failInterval);
        }
    }
}
