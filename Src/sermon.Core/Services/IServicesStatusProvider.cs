using sermon.Core.Entities;

namespace sermon.Core.Services
{
    public interface IServicesStatusProvider
    {
        ServiceStatus[] Get();
    }
}