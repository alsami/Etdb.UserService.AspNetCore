using EntertainmentDatabase.REST.API.Misc.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace EntertainmentDatabase.REST.API.Misc.Filters
{
    public class RegisterExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is RegisterException))
            {
                return;
            }

            context.Result = new BadRequestObjectResult((context.Exception as RegisterException).IdentityErrors);
            //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.ExceptionHandled = true;
        }
    }
}
