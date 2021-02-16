using sermon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace sermon.Core.Services
{
    public interface IServicesInfoProvider
    {
        ServiceInfo[] Get();
    }
}
