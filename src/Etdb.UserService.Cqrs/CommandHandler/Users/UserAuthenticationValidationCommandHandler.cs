using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Abstractions.Events.Users;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Enums;
using Etdb.UserService.Services.Abstractions;
using MediatR;

#nullable enable

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class UserAuthenticationValidationCommandHandler :
        IRequestHandler<UserAuthenticationValidationCommand, AuthenticationValidationDto>
    {
        private readonly IMediator bus;
        private readonly IHasher hasher;
        private readonly IUsersService usersService;

        public UserAuthenticationValidationCommandHandler(IUsersService usersService, IHasher hasher, IMediator bus)
        {
            this.usersService = usersService;
            this.hasher = hasher;
            this.bus = bus;
        }

        public async Task<AuthenticationValidationDto> Handle(UserAuthenticationValidationCommand command,
            CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByUserNameOrEmailAsync(command.UserName);

            if (user is null)
                return AuthenticationValidationDto.FailedAuthentication(AuthenticationFailure.Unavailable);

            var passwordIsValid = this.hasher.CreateSaltedHash(command.Password, user.Salt!) == user.Password;

            if (!passwordIsValid)
            {
                var authenticationFailureEvent = new UserAuthenticatedEvent(user.Id, user.UserName,
                    AuthenticationLogType.Failed.ToString(), command.IpAddress, DateTime.UtcNow,
                    "Given password is invalid!");

                await this.bus.Publish(authenticationFailureEvent, cancellationToken);

                return AuthenticationValidationDto.FailedAuthentication(AuthenticationFailure.InvalidPassword);
            }

            if (user.IsLocked())
                return AuthenticationValidationDto.FailedAuthentication(AuthenticationFailure.LockedOut);

            var authenticationSucceededEvent = new UserAuthenticatedEvent(user.Id, user.UserName,
                AuthenticationLogType.Succeeded.ToString(), command.IpAddress, DateTime.UtcNow);

            await this.bus.Publish(authenticationSucceededEvent, cancellationToken);

            return new AuthenticationValidationDto(true, userId: user.Id);
        }
    }
}