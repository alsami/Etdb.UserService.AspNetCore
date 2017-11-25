using System;
using System.Collections.Generic;
using System.Text;
using ETDB.API.UserService.Domain.Events;
using MediatR;

namespace ETDB.API.UserService.Domain.Handler
{
    public class UserEventHandler : INotificationHandler<UserRegisterEvent>
    {
        public void Handle(UserRegisterEvent notification)
        {
            Console.WriteLine();
        }
    }
}
