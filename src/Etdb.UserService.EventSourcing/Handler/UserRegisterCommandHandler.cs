using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.ServiceBase.EventSourcing.Abstractions.Notifications;
using Etdb.ServiceBase.EventSourcing.Handler;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.ServiceBase.General.Abstractions.Hasher;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.EventSourcing.Events;
using Etdb.UserService.EventSourcing.Validation;
using Etdb.UserService.Presentation.DTO;
using Etdb.UserService.Repositories.Abstractions;
using FluentValidation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Etdb.UserService.EventSourcing.Handler
{
    public class UserRegisterCommandHandler : TransactionHandler<UserRegisterCommand, UserDTO>
    {
        private readonly IUserRepository userRepository;
        private readonly IHasher hasher;
        private readonly IMapper mapper;
        private readonly UserRegisterCommandValidation validation;

        public UserRegisterCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediator, IDomainNotificationHandler<DomainNotification> notificationsHandler, IUserRepository userRepository, IHasher hasher, UserRegisterCommandValidation validation, IMapper mapper) : base(unitOfWork, mediator, notificationsHandler)
        {
            this.userRepository = userRepository;
            this.hasher = hasher;
            this.validation = validation;
            this.mapper = mapper;
        }

        public override Task<UserDTO> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            if (!this.validation.IsValid(request, out var validationResult))
            {
                this.NotifyValidationErrors(request, validationResult);

                throw new CommandValidationException("Error validating user registration", 
                    validationResult.Errors.Select(error => error.ErrorMessage).ToArray());
            }

            var salt = this.hasher.GenerateSalt();

            var user = new User(Guid.NewGuid(), request.Name, request.LastName, request.Email,
                request.UserName, this.hasher.CreateSaltedHash(request.Password, salt), salt);

            this.userRepository.Register(user);

            if (this.CanCommit())
            {
                this.Mediator.RaiseEvent(new UserRegisterEvent(user.Id, user.Name, user.LastName, user.Email, user.UserName,
                    user.Password, user.Salt, user.RowVersion, user.UserSecurityroles));

                return Task.FromResult(this.mapper.Map<UserDTO>(user));
            }

            this.Mediator.RaiseEvent(new DomainNotification(request.MessageType,
                "The email or username has already been taken."));

            // TODO
            throw new NotImplementedException("TODO");
        }
    }
}
