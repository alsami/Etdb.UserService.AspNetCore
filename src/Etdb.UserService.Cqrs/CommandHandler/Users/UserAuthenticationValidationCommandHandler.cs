using System;
using System.Net;
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
        private readonly IUsersService usersService;
        private readonly IHasher hasher;
        private readonly IMediator bus;

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

            if (user == null)
            {
                return FailedAuthentication(AuthenticationFailure.Unavailable);
            }

            var passwordIsValid = this.hasher.CreateSaltedHash(command.Password, user.Salt!)
                                  == user.Password;

            if (!passwordIsValid)
            {
                await this.PublishAuthenticationEvent(user.Id,  user.UserName, AuthenticationLogType.Failed, command.IpAddress,
                    "Given password is invalid!",
                    cancellationToken);

                return FailedAuthentication(AuthenticationFailure.InvalidPassword);
            }

            if (await this.usersService.IsUserLocked(user.Id))
            {
                return FailedAuthentication(AuthenticationFailure.LockedOut);
            }

            await this.PublishAuthenticationEvent(user.Id, user.UserName, AuthenticationLogType.Succeeded, command.IpAddress,
                cancellationToken: cancellationToken);

            return new AuthenticationValidationDto(true, userId: user.Id);
        }

        private static AuthenticationValidationDto FailedAuthentication(AuthenticationFailure authenticationFailure)
            => new AuthenticationValidationDto(false, authenticationFailure);

        private Task PublishAuthenticationEvent(
            Guid userId, string userName,
            AuthenticationLogType authenticationLogType,
            IPAddress ipAddress,
            string? additionalInfo = null, CancellationToken cancellationToken = default)
            => this.bus.Publish(
                new UserAuthenticatedEvent(userId, userName, authenticationLogType.ToString(), ipAddress , DateTime.UtcNow,
                    additionalInfo),
                cancellationToken);
    }
}