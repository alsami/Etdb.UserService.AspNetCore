using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class ContextLessRouteProvider
    {
        public virtual IRouter Router { get; set; }
    }
}