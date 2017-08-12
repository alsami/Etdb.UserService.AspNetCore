using EntertainmentDatabase.REST.API.Misc.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntertainmentDatabase.REST.API.Misc.Filters
{
    public class RessourceNotFoundFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is RessourceNotFoundException))
            {
                return;
            }

            context.Result = new NotFoundObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}
