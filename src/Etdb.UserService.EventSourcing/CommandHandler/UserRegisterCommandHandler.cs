using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.EventSourcing.Abstractions.Bus;
using Etdb.ServiceBase.EventSourcing.Abstractions.Handler;
using Etdb.ServiceBase.General.Abstractions.Exceptions;
using Etdb.ServiceBase.General.Abstractions.Hasher;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.EventSourcing.Events;
using Etdb.UserService.EventSourcing.Validation;
using Etdb.UserService.Presentation.DataTransferObjects;
using Etdb.UserService.Repositories.Abstractions;
using MongoDB.Driver;

namespace Etdb.UserService.EventSourcing.CommandHandler
{
    public class UserRegisterCommandHandler : ITransactionHandler<UserRegisterCommand, UserDto>
    {
        private readonly IMediatorHandler mediator;
        private readonly IUserRepository userRepository;
        private readonly ISecurityRoleRepository securityRoleRepository;
        private readonly IHasher hasher;
        private readonly IMapper mapper;
        private readonly UserRegisterCommandValidation validation;

        public UserRegisterCommandHandler(IMediatorHandler mediator,
            IUserRepository userRepository, IHasher hasher, UserRegisterCommandValidation validation, 
            IMapper mapper, ISecurityRoleRepository securityRoleRepository)
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
            this.hasher = hasher;
            this.validation = validation;
            this.mapper = mapper;
            this.securityRoleRepository = securityRoleRepository;
        }

        public async Task<UserDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            if (!this.validation.IsValid(request, out var validationResult))
            {
                throw new CommandValidationException("Error validating user registration", 
                    validationResult.Errors.Select(error => error.ErrorMessage).ToArray());
            }

            var user = this.mapper.Map<User>(request);

            var salt = this.hasher.GenerateSalt();
            user.Password = this.hasher.CreateSaltedHash(request.Password, salt);
            user.Salt = salt;

            var memberRole = await this.securityRoleRepository.FindAsync(RoleNames.Member);

            user.SecurityRoles.Add(new MongoDBRef($"{typeof(SecurityRole).Name}s", memberRole.Id));

            await this.userRepository.RegisterAsync(user);

            await this.mediator.RaiseEvent(this.mapper.Map<UserRegisterEvent>(user));

            return this.mapper.Map<UserDto>(user);
        }
    }
}
