using System;
using System.Runtime.CompilerServices;
using ETDB.API.ServiceBase.Abstractions.Hasher;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Base;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Bus;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Handler;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Notifications;
using ETDB.API.ServiceBase.EventSourcing.Abstractions.Validation;
using ETDB.API.ServiceBase.EventSourcing.Handler;
using ETDB.API.UserService.Domain.Entities;
using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.EventSourcing.Events;
using ETDB.API.UserService.EventSourcing.Validation;
using ETDB.API.UserService.Repositories.Repositories;

namespace ETDB.API.UserService.EventSourcing.Handler
{
    public class UserRegisterCommandHandler : CommandHandler<UserRegisterCommand>
    {
        private readonly UserRegisterCommandValidation commandValidation;
        private readonly IUserRepository userRepository;
        private readonly IHasher hasher;

        public UserRegisterCommandHandler(UserRegisterCommandValidation commandValidation,
            IUserRepository userRepository, IHasher hasher,
            IUnitOfWork unitOfWork, IMediatorHandler bus, IDomainNotificationHandler<DomainNotification> notificationsHandler) 
                : base( unitOfWork, bus, notificationsHandler)
        {
            this.commandValidation = commandValidation;
            this.userRepository = userRepository;
            this.hasher = hasher;
        }

        public override void Handle(UserRegisterCommand notification)
        {
            if (!this.commandValidation.IsValid(notification, out var validationResult))
            {
                this.NotifyValidationErrors(notification, validationResult);
                return;
            }

            var salt = this.hasher.GenerateSalt();

            var user = new User(Guid.NewGuid(), notification.Name, notification.LastName, notification.Email,
                notification.UserName, this.hasher.CreateSaltedHash(notification.Password, salt), salt);

            this.userRepository.Register(user);

            if (this.Commit())
            {
                this.Bus.RaiseEvent(new UserRegisterEvent(user.Id, user.Name, user.LastName, user.Email, user.UserName,
                    user.Password, user.Salt, user.RowVersion, user.UserSecurityroles));

                return;
            }

            this.Bus.RaiseEvent(new DomainNotification(notification.MessageType,
                "The email or username has already been taken."));
        }
    }
}
