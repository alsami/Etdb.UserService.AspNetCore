using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.UserService.Controllers.V1
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    public class DummyController : Controller
    {
        [HttpGet]
        public IActionResult Test() => this.Ok();
    }
}