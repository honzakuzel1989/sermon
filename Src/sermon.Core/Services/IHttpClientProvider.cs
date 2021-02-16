using System.Net.Http;

namespace sermon.Core.Services
{
    public interface IHttpClientProvider
    {
        HttpClient Get();
    }
}