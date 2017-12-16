using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Application.ExceptionFilter
{
    public class GeneralExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GeneralExceptionFilter> logger;

        public GeneralExceptionFilter(ILogger<GeneralExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            this.logger.LogError(context.Exception.Message, context.Exception);

            if (context.ExceptionHandled)
            {
                return;
            }

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new
            {
                Message = "An unknown server error occured",
                context.Exception
            });
        }
    }
}
