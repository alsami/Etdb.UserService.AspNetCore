using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.EventSourcing.Events;
using Etdb.UserService.EventSourcing.Validation;
using Etdb.UserService.Presentation.DataTransferObjects;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.CommandHandler
{
    public class UserUpdateCommandHandler : ITransactionHandler<UserUpdateCommand, UserDto>
    {
        private readonly IMediatorHandler mediator;
        private readonly IUserRepository userRepository;
        private readonly UserUpdateCommandValidation validation;
        private readonly IMapper mapper;

        public UserUpdateCommandHandler(IMediatorHandler mediator, IUserRepository userRepository, UserUpdateCommandValidation validation, IMapper mapper)
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
            this.validation = validation;
            this.mapper = mapper;
        }

        public async Task<UserDto> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            if (!this.validation.IsValid(request, out var validationResult))
            {
                throw new CommandValidationException("Error validating user update request", 
                    validationResult.Errors.Select(error => error.ErrorMessage).ToArray());
            }

            var existingUser = await this.userRepository.GetAsync(request.Id);

            if (existingUser == null)
            {
                throw new ResourceNotFoundException($"The user with the Id {request.Id} was not found");
            }

            if (!existingUser.RowVersion.SequenceEqual(request.RowVersion))
            {
                throw new ConcurrencyException("The requested user record has been updated before you tried to update it!",
                    this.mapper.Map<UserDto>(existingUser));
            }

            this.mapper.Map(request, existingUser);

            if (!await this.userRepository.EditAsync(existingUser))
            {
                // TODO
                throw new Exception("TODO");
            }

            await this.mediator.RaiseEvent(new UserUpdateEvent(existingUser.Id, existingUser.Name, existingUser.LastName,
                existingUser.Email, existingUser.UserName));

            return this.mapper.Map<UserDto>(existingUser);
        }
    }
}
