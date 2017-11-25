using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETDB.API.UserService.Admin.Controllers.v1
{
    public abstract class ApiController : ControllerBase
    {
        private readonly INotificationHandler<DomainNotification> notificationHandler;

        protected ApiController(INotificationHandler<DomainNotification> notifications)
        {
            notificationHandler = notifications;
        }

        protected IEnumerable<DomainNotification> Notifications 
            => ((DomainNotificationHandler<DomainNotification>)notificationHandler).GetNotifications();

        protected bool IsValidOperation()
        {
            return !((DomainNotificationHandler<DomainNotification>)notificationHandler).HasNotifications();
        }

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
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
                errors = ((DomainNotificationHandler<DomainNotification>)notificationHandler)
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
