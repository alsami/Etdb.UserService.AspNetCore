using System;
using System.Diagnostics;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntertainmentDatabase.REST.API.Bootstrap.Filters
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
                lock (this) { 
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
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
