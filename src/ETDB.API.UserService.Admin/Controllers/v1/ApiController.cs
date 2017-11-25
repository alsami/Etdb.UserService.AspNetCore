using System.Collections.Generic;
using System.Linq;
using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ETDB.API.UserService.Admin.Controllers.v1
{
    public abstract class ApiController : ControllerBase
    {
        private readonly IDomainNotificationHandler<DomainNotification> notificationHandler;

        protected ApiController(IDomainNotificationHandler<DomainNotification> notifications)
        {
            notificationHandler = notifications;
        }

        protected IEnumerable<DomainNotification> Notifications 
            => notificationHandler.GetNotifications();

        protected bool IsValidOperation()
        {
            return !notificationHandler.HasNotifications();
        }

        protected new IActionResult Response(object result = null)
        {
            if (this.IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = notificationHandler
                    .GetNotifications().Select(n => n.Value)
            });
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            notificationHandler.Handle(new DomainNotification(code, message));
        }
    }
}
