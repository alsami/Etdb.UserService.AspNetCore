using System;
using System.Diagnostics;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntertainmentDatabase.REST.API.WebService.Misc.Filters
{
    public class ErrorLogFilter : IExceptionFilter
    {
        private readonly IEntityRepository<ErrorLog> errorLogRepository;

        public ErrorLogFilter(IEntityRepository<ErrorLog> errorLogRepository)
        {
            this.errorLogRepository = errorLogRepository;
        }

        public void OnException(ExceptionContext context)
        {
            try
            {
                this.errorLogRepository.Add(new ErrorLog
                {
                    Occurrence = DateTime.UtcNow,
                    HttpMethod = context.HttpContext.Request.Method,
                    Message = context.Exception.Message,
                    TraceId = context.HttpContext.TraceIdentifier,
                    Path = context.HttpContext.Request.Path
                });
                this.errorLogRepository.EnsureChanges();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
