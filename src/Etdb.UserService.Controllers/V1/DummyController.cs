using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DummyController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test() => this.Ok();
    }
}