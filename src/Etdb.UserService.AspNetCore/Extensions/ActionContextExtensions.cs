using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.AspNetCore.Extensions
{
    public static class ActionContextExtensions
    {
        public static bool TryParseRouteParameterId(this HttpContext context, string key, out Guid uid)
            => Guid.TryParse(context.GetRouteValue(key)?.ToString(), out uid);
    }
}