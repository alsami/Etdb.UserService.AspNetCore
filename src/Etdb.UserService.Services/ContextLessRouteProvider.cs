using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class ContextLessRouteProvider
    {
        public IRouter Router { get; set; }
    }
}