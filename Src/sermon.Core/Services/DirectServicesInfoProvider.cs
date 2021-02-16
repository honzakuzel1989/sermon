using Microsoft.Extensions.Logging;
using sermon.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace sermon.Core.Services
{
    public class DirectServicesInfoProvider : IServicesInfoProvider
    {
        private const string DEFAULT_CUSTOM_SERVICE_BASE_URL = "http://localhost";

        private readonly ConcurrentBag<ServiceInfo> _serviceInfos;
        private readonly ILogger<DirectServicesInfoProvider> _logger;

        public DirectServicesInfoProvider(ILogger<DirectServicesInfoProvider> logger)
        {
            _serviceInfos = new ConcurrentBag<ServiceInfo>();
            _logger = logger;

            var bu = Environment.GetEnvironmentVariable("SERMON_CUSTOM_SERVICE_BASE_URL")
                ?? DEFAULT_CUSTOM_SERVICE_BASE_URL;

            Add(new ServiceInfo("prehdo", $"{bu}:5000/prehdo"));
            Add(new ServiceInfo("datetime", $"{bu}:5005/datetime"));
            Add(new ServiceInfo("weather", $"{bu}:5010/weather"));
            Add(new ServiceInfo("location", $"{bu}:5015/location"));
            Add(new ServiceInfo("system", $"{bu}:5020/system"));
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
