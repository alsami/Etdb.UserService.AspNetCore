using Etdb.UserService.Misc.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Filter
{
    public class IdentityServerExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<IdentityServerExceptionFilter> logger;

        public IdentityServerExceptionFilter(ILogger<IdentityServerExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is IdentityServerException))
            {
                return;
            }

            this.logger.LogError(context.Exception, context.Exception.Message);

            context.ExceptionHandled = true;
            context.Result = new UnauthorizedObjectResult(new
            {
                Message = context.Exception.Message
            });
        }
    }
}