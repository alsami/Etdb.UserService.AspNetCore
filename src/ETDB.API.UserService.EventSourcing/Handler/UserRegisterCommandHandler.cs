using System;
using ETDB.API.ServiceBase.Abstractions.Handler;
using ETDB.API.ServiceBase.Domain.Abstractions.Base;
using ETDB.API.ServiceBase.Domain.Abstractions.Bus;
using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.EventSourcing.Events;
using ETDB.API.UserService.Repositories.Repositories;

namespace ETDB.API.UserService.EventSourcing.Handler
{
    public class UserRegisterCommandHandler : CommandHandler<UserRegisterCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IMediatorHandler bus;

        public UserRegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
            IMediatorHandler bus, IDomainNotificationHandler<DomainNotification> notificationsHandler) 
                : base( unitOfWork, bus, notificationsHandler)
        {
            this.bus = bus;
            this.userRepository = userRepository;
        }

        public override void Handle(UserRegisterCommand notification)
        {
            if (!notification.IsValid())
            {
                // TODO
                Console.WriteLine("NOT VALID");
            }

            var user = new User(Guid.NewGuid(), notification.Name, notification.LastName, notification.Email,
                notification.UserName, notification.Password, notification.Salt);

            if (this.userRepository.Get(user.UserName, user.Email) == null)
            {
                this.userRepository.Register(user);

                if (this.Commit())
                {
                    this.bus.RaiseEvent(new UserRegisterEvent(user.Id, user.Name, user.LastName, user.Email, user.UserName,
                        user.Password, user.Salt, user.RowVersion, user.UserSecurityroles));

                    return;
                }
            }

            bus.RaiseEvent(new DomainNotification(notification.MessageType,
                "The email or username has already been taken."));
        }
    }
}
