using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EntertainmentDatabase.REST.API.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class RPCController : Controller
    {
        private readonly IConfigurationRoot configurationRoot;

        public RPCController(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }

        [HttpGet("reset")]
        public IActionResult ReloadAppSettings()
        {
            this.configurationRoot.Reload();
            return Ok();
        }
    }
}
