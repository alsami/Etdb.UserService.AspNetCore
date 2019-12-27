using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Etdb.UserService.AspNetCore.Extensions
{
    public static class ActionDescriptorExtensions
    {
        public static bool AllowsAnonymousForControllerOrAction(this ActionDescriptor actionDescriptor,
            bool withInheritance = true)
            => actionDescriptor.AllowsAnonymousAccessForAction(withInheritance) ||
               actionDescriptor.AllowsAnonymousForController(withInheritance);

        // ReSharper disable twice MemberCanBePrivate.Global
        public static bool AllowsAnonymousAccessForAction(this ActionDescriptor actionDescriptor,
            bool withInheritance = true)
            => ((ControllerActionDescriptor) actionDescriptor).MethodInfo.IsDefined(typeof(AllowAnonymousAttribute),
                withInheritance);

        public static bool AllowsAnonymousForController(this ActionDescriptor actionDescriptor,
            bool withInheritance = true)
            => ((ControllerActionDescriptor) actionDescriptor).ControllerTypeInfo.IsDefined(
                typeof(AllowAnonymousAttribute),
                withInheritance);
    }
}