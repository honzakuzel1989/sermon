using Microsoft.Extensions.Logging;
using sermon.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace sermon.Core.Services
{
    public class DirectServicesInfoProvider : IServicesInfoProvider
    {
        // Format: name1=whole_url1;name2=whole_url2;...;nameN=whole_utlN
        private const string DEFAULT_CUSOM_SERVICES = @"
            prehdo=http://localhost:5000/prehdo;
            datetime=http://localhost:5005/datetime;
            weather=http://localhost:5010/weather;
            location=http://localhost:5015/location;
            system=http://localhost:5020/system;
            weatherhub=http://localhost:5025/weatherhub;
            covid=http://localhost:5030/covid";

        private readonly ConcurrentBag<ServiceInfo> _serviceInfos;
        private readonly ILogger<DirectServicesInfoProvider> _logger;

        public DirectServicesInfoProvider(ILogger<DirectServicesInfoProvider> logger)
        {
            _serviceInfos = new ConcurrentBag<ServiceInfo>();
            _logger = logger;

            var cs = Environment.GetEnvironmentVariable("SERMON_CUSTOM_SERVICES")
                ?? DEFAULT_CUSOM_SERVICES;

            var services = cs.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var service in services)
            {
                var serviceInfo = service.Split('=', StringSplitOptions.RemoveEmptyEntries);

                Add(new ServiceInfo(serviceInfo[0].Trim(), serviceInfo[1].Trim()));
            }
        }

        public void Add(ServiceInfo serviceInfo)
        {
            _logger.LogInformation($"Adding service {serviceInfo}..");

            if (_serviceInfos.Any(si => si.Name == serviceInfo.Name))
                throw new ArgumentException($"Service info with name {serviceInfo.Name} already exists!");

            _serviceInfos.Add(serviceInfo);
        }

        public ServiceInfo[] Get()
        {
            return _serviceInfos.ToArray();
        }
    }
}
