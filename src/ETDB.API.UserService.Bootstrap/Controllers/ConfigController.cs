using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETDB.API.UserService.Presentation.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ETDB.API.UserService.Bootstrap.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        private readonly IConfigurationRoot configuration;

        public ConfigController(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("client")]
        public WebClientConfigDTO GetClientConfig()
        {
            return new WebClientConfigDTO
            {
                ClientName = this.configuration.GetSection("IdentityConfig")
                    .GetSection("WebClient")
                    .GetValue<string>("Name"),

                Secret = configuration.GetSection("IdentityConfig")
                    .GetSection("WebClient")
                    .GetValue<string>("Secret")
            };
        }
    }
}
