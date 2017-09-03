using EntertainmentDatabase.REST.API.Misc.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntertainmentDatabase.REST.API.Misc.Filters
{
    public class LoginFailedExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is LoginFailedException)
            {
                context.Result = new BadRequestObjectResult(context.Exception);
                context.ExceptionHandled = true;
            }

            return;
        }
    }
}
