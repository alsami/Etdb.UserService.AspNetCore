//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
//using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
//using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
//using Etdb.ServiceBase.EventSourcing.Abstractions.Notifications;
//using Etdb.ServiceBase.EventSourcing.Handler;
//using Etdb.ServiceBase.General.Abstractions.Hasher;
//using Etdb.UserService.Domain.Entities;
//using Etdb.UserService.EventSourcing.Commands;
//using Etdb.UserService.EventSourcing.Events;
//using Etdb.UserService.EventSourcing.Validation;
//using Etdb.UserService.Repositories.Abstractions;

//namespace Etdb.UserService.EventSourcing.Handler
//{
//    public class UserRegisterCommandHandlerOld : CommandHandler<UserRegisterCommandOldOld>
//    {
//        private readonly UserRegisterCommandValidation commandValidation;
//        private readonly IUserRepository userRepository;
//        private readonly IHasher hasher;

//        public UserRegisterCommandHandlerOld(UserRegisterCommandValidation commandValidation,
//            IUserRepository userRepository, IHasher hasher,
//            IUnitOfWork unitOfWork, IMediatorHandler mediator, IDomainNotificationHandler<DomainNotification> notificationsHandler) 
//                : base( unitOfWork, mediator, notificationsHandler)
//        {
//            this.commandValidation = commandValidation;
//            this.userRepository = userRepository;
//            this.hasher = hasher;
//        }

//        public override Task Handle(UserRegisterCommandOldOld notification, CancellationToken cancellationToken)
//        {
//            if (!this.commandValidation.IsValid(notification, out var validationResult))
//            {
//                this.NotifyValidationErrors(notification, validationResult);
//                return Task.FromResult(1);
//            }

//            var salt = this.hasher.GenerateSalt();

//            var user = new User(Guid.NewGuid(), notification.Name, notification.LastName, notification.Email,
//                notification.UserName, this.hasher.CreateSaltedHash(notification.Password, salt), salt);

//            this.userRepository.Register(user);

//            if (this.Commit())
//            {
//                this.Mediator.RaiseEvent(new UserRegisterEvent(user.Id, user.Name, user.LastName, user.Email, user.UserName,
//                    user.Password, user.Salt, user.RowVersion, user.UserSecurityroles));

//                return Task.FromResult(0);
//            }

//            this.Mediator.RaiseEvent(new DomainNotification(notification.MessageType,
//                "The email or username has already been taken."));

//            return Task.FromResult(0);
//        }
//    }
//}
