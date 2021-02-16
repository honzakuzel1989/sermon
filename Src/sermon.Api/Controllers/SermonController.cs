using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sermon.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sermon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SermonController : ControllerBase
    {
        private const string DEFAULT_FORMAT = "dd.MM. HH:mm";

        private readonly ILogger<SermonController> _logger;
        private readonly IServicesStatusProvider _servicesStatusProvider;

        public SermonController(ILogger<SermonController> logger,
            IServicesStatusProvider servicesStatusProvider)
        {
            _logger = logger;
            _servicesStatusProvider = servicesStatusProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(";)");
        }

        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            var ss = _servicesStatusProvider.Get();

            return new JsonResult(new
            {
                Timestamp = DateTime.Now,
                Services = ss.Select(status =>
                new
                {
                    Name = status.ServiceRecord.ServiceInfo.Name,
                    Url = status.ServiceRecord.ServiceInfo.Url,
                    Timestamp = status.ServiceRecord.Timestamp,
                    Alive = status.Alive,
                    Title = status.Alive ? status.ServiceRecord.Timestamp?.ToString(DEFAULT_FORMAT) : "💀",
                })
            });
        }

        [HttpGet("allok")]
        public IActionResult GetAllOk()
        {
            var ss = _servicesStatusProvider.Get();
            var allok = ss.All(s => s.Alive);
            var t = DateTime.Now;

            return new JsonResult(new
            {
                Timestamp = t,
                AllOk = allok,
                Title = allok
                    ? $"{t.ToString(DEFAULT_FORMAT)} AllOk"
                    : string.Join(',', ss.Where(s => !s.Alive).Select(s => s.ServiceRecord.ServiceInfo.Name)) + " down"
            });
        }
    }
}
