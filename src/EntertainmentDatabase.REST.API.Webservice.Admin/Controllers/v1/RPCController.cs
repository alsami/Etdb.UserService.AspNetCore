using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EntertainmentDatabase.REST.API.WebService.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class RPCController : Controller
    {
        private readonly ILogger<RPCController> logger;
        private readonly IConfigurationRoot configurationRoot;

        public RPCController(ILogger<RPCController> logger, IConfigurationRoot configurationRoot)
        {
            this.logger = logger;
            this.configurationRoot = configurationRoot;
        }

        [HttpGet("settings-reload")]
        public IActionResult ReloadAppSettings()
        {
            this.logger.LogInformation("Reloading app settings!");
            this.configurationRoot.Reload();
            this.logger.LogInformation("Reloading app settings done!");
            return Ok();
        }
    }
}
