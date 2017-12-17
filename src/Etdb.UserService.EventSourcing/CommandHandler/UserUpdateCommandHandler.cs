using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.EventSourcing.Handler;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.EventSourcing.Events;
using Etdb.UserService.EventSourcing.Validation;
using Etdb.UserService.Presentation.DTO;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.CommandHandler
{
    public class UserUpdateCommandHandler : TransactionHandler<UserUpdateCommand, UserDTO>
    {
        private readonly IUserRepository userRepository;
        private readonly UserUpdateCommandValidation validation;
        private readonly IMapper mapper;

        public UserUpdateCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediator, IUserRepository userRepository, UserUpdateCommandValidation validation, IMapper mapper) : base(unitOfWork, mediator)
        {
            this.userRepository = userRepository;
            this.validation = validation;
            this.mapper = mapper;
        }

        public override Task<UserDTO> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            if (!this.validation.IsValid(request, out var validationResult))
            {
                throw new CommandValidationException("Error validating user update request", 
                    validationResult.Errors.Select(error => error.ErrorMessage).ToArray());
            }

            var existingUser = this.userRepository.FindWithIncludes(request.Id);

            if (existingUser == null)
            {
                throw new ResourceNotFoundException($"The user with the Id {request.Id} was not found");
            }

            if (!existingUser.RowVersion.SequenceEqual(request.RowVersion))
            {
                throw new ConcurrencyException("The requested user record has been updated before you tried to update it!",
                    this.mapper.Map<UserDTO>(existingUser));
            }

            this.mapper.Map(request, existingUser);

            if (!this.CanCommit(out var eventstreamException))
            {
                throw eventstreamException;
            }

            this.Mediator.RaiseEvent(new UserUpdateEvent(existingUser.Id, existingUser.Name, existingUser.LastName,
                existingUser.Email, existingUser.UserName, existingUser.Password, existingUser.Salt,
                existingUser.RowVersion,
                existingUser.UserSecurityroles));

            return Task.FromResult(this.mapper.Map<UserDTO>(existingUser));
        }
    }
}
