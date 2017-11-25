using System;
using System.Collections.Generic;
using System.Text;
using ETDB.API.ServiceBase.Domain.Abstractions.Base;
using ETDB.API.ServiceBase.Domain.Abstractions.Bus;
using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using ETDB.API.ServiceBase.Handler;
using ETDB.API.UserService.Domain.Commands;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.Domain.Events;
using MediatR;

namespace ETDB.API.UserService.Domain.Handler
{
    public class UserCommandHandler : CommandHandler, INotificationHandler<RegisterUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IMediatorHandler bus;

        public UserCommandHandler(IUserRepository userRepository,
            IUnitOfWork unitOfWork, IMediatorHandler bus, 
            INotificationHandler<DomainNotification> notificationsHandler) 
            : base(unitOfWork, bus, notificationsHandler)
        {
            this.bus = bus;
            this.userRepository = userRepository;
        }

        public void Handle(RegisterUserCommand notification)
        {
            if (!notification.IsValid())
            {
                // TODO
                Console.WriteLine("NOT VALID");
            }

            var user = new User(Guid.NewGuid(), notification.Name, notification.LastName, notification.Email,
                notification.UserName, notification.Password, notification.Salt);

            if (this.userRepository.Get(user.UserName, user.Email) != null)
            {
                bus.RaiseEvent(new DomainNotification(notification.MessageType, 
                    "The email or username has already been taken."));
            }

            this.userRepository.Register(user);

            if (this.Commit())
            {
                this.bus.RaiseEvent(new UserRegisterEvent(user.Id, user.Name, user.LastName, user.Email, user.UserName,
                    user.Password, user.Salt));
            }
        }
    }
}
