using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sermon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace sermon.Core.Services
{
    public class SermonHosedService : IHostedService, IDisposable
    {
        private const int DEFAULT_CHECK_INTERVAL_S = 5;

        private bool disposedValue;
        private System.Timers.Timer _timer;
        
        private readonly ILogger<SermonHosedService> _logger;
        private readonly IServicesInfoProvider _servicesInfoProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        private readonly int _timerInterval;

        public SermonHosedService(ILogger<SermonHosedService> logger,
            IHttpClientProvider httpClientProvider,
            IServicesInfoProvider servicesInfoProvider,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _servicesInfoProvider = servicesInfoProvider;
            _memoryCache = memoryCache;
            _httpClient = httpClientProvider.Get();

            _timerInterval = int.TryParse(Environment.GetEnvironmentVariable("SERMON_CHECK_INTERVAL_S"), out var s)
                ? s
                : DEFAULT_CHECK_INTERVAL_S;
            _logger.LogInformation($"_timerInterval = {_timerInterval}s");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"SermonHosedService running... Timeout {_timerInterval}s.");
            
            _timer = new System.Timers.Timer(TimeSpan.FromSeconds(_timerInterval).TotalMilliseconds);
            _timer.Elapsed += DoCheck;
            _timer.AutoReset = false;
            _timer.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SermonHosedService is stopping.");
            _timer?.Stop();

            return Task.CompletedTask;
        }

        private void DoCheck(object state, ElapsedEventArgs eeh)
        {
            try
            {
                _logger.LogInformation("SermonHosedService checking services.");
                foreach (var serviceInfo in _servicesInfoProvider.Get())
                {
                    try
                    {
                        _httpClient.GetStringAsync(serviceInfo.Url).GetAwaiter().GetResult();
                        _memoryCache.Set(serviceInfo.Name, new ServiceRecord(serviceInfo, DateTime.Now));

                        _logger.LogInformation($"Result from url {serviceInfo.Url} successfully obtained");
                    }
                    catch
                    {
                        _logger.LogError($"Cannot get result from url {serviceInfo.Url}");
                    }
                }
            }
            finally
            {
                _timer.Start();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
