using System;
using System.Net.Http;

namespace sermon.Core.Services
{
    public class HttpClientProvider : IHttpClientProvider, IDisposable
    {
        private readonly HttpClient httpClient;
        private bool disposedValue;

        public HttpClientProvider()
        {
            httpClient = new HttpClient();
        }

        public HttpClient Get()
        {
            return httpClient;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    httpClient.Dispose();
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
