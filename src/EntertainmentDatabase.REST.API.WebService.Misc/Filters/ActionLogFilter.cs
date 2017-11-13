using System;
using System.Diagnostics;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EntertainmentDatabase.REST.API.WebService.Misc.Filters
{
    public class ActionLogFilter : IActionFilter, IDisposable
    {
        private readonly IEntityRepository<ActionLog> actionLogRepository;
        private DateTime traceStart;
        private readonly Stopwatch stopwatch;

        public ActionLogFilter(IEntityRepository<ActionLog> actionLogRepository)
        {
            this.actionLogRepository = actionLogRepository;
            this.stopwatch = new Stopwatch();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            traceStart = DateTime.UtcNow;
            stopwatch.Start();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this.stopwatch.Stop();

            this.actionLogRepository.Add(new ActionLog
            {
                TraceId = context.HttpContext.TraceIdentifier,
                TraceStart = this.traceStart,
                TraceEnd = this.traceStart.AddMilliseconds(this.stopwatch.ElapsedMilliseconds),
                HttpMethod = context.HttpContext.Request.Method,
                Path = context.HttpContext.Request.Path,
            });

            try
            {
                this.actionLogRepository.EnsureChanges();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        public void Dispose()
        {
            if (this.stopwatch.IsRunning)
            {
                this.stopwatch.Stop();
            }
        }
    }
}
