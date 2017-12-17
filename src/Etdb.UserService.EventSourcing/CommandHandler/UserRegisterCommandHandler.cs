using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.EventSourcing.Handler;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.ServiceBase.General.Abstractions.Hasher;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.EventSourcing.Events;
using Etdb.UserService.EventSourcing.Validation;
using Etdb.UserService.Presentation.DTO;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.CommandHandler
{
    public class UserRegisterCommandHandler : TransactionHandler<UserRegisterCommand, UserDTO>
    {
        private readonly IUserRepository userRepository;
        private readonly IHasher hasher;
        private readonly IMapper mapper;
        private readonly UserRegisterCommandValidation validation;

        public UserRegisterCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediator,
            IUserRepository userRepository, IHasher hasher, UserRegisterCommandValidation validation, 
            IMapper mapper) : base(unitOfWork, mediator)
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
                throw new CommandValidationException("Error validating user registration", 
                    validationResult.Errors.Select(error => error.ErrorMessage).ToArray());
            }

            var salt = this.hasher.GenerateSalt();

            var user = this.mapper.Map<User>(request);
            user.Password = this.hasher.CreateSaltedHash(request.Password, salt);
            user.Salt = salt;

            this.userRepository.Register(user);

            if (!this.CanCommit(out var saveEventstreamException))
            {
                throw saveEventstreamException;
            }

            this.Mediator.RaiseEvent(new UserRegisterEvent(user.Id, user.Name, user.LastName, user.Email, user.UserName,
                user.Password, user.Salt, user.RowVersion, user.UserSecurityroles));

            return Task.FromResult(this.mapper.Map<UserDTO>(user));
        }
    }
}
