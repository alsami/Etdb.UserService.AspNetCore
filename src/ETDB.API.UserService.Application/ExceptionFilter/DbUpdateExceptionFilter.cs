using Microsoft.AspNetCore.Mvc.Filters;

namespace Etdb.UserService.Application.ExceptionFilter
{
    public class DbUpdateExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //if (!(context.Exception is DbUpdateException))
            //{
            //    return;
            //}

            //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.Result = new ObjectResult(context.Exception);
            //context.ExceptionHandled = true;
        }
    }
}
