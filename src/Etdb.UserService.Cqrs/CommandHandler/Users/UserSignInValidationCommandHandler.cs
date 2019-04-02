using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Abstractions.Events.Authentication;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Authentication;
using Etdb.UserService.Presentation.Enums;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class UserSignInValidationCommandHandler :
        IResponseCommandHandler<UserSignInValidationCommand, SignInValidationDto>
    {
        private readonly IUsersService usersService;
        private readonly IHasher hasher;
        private readonly IBus bus;

        public UserSignInValidationCommandHandler(IUsersService usersService, IHasher hasher, IBus bus)
        {
            this.usersService = usersService;
            this.hasher = hasher;
            this.bus = bus;
        }

        public async Task<SignInValidationDto> Handle(UserSignInValidationCommand command,
            CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByUserNameOrEmailAsync(command.UserName);

            if (user == null)
            {
                return FailedSignIn(SignInFailure.Unavailable);
            }

            var passwordIsValid = this.hasher.CreateSaltedHash(command.Password, user.Salt)
                                  == user.Password;

            if (!passwordIsValid)
            {
                await this.PublishSignInEvent(SignInType.Failed, user.Id, command.IpAddress, "Given password is invalid!",
                    cancellationToken);

                return FailedSignIn(SignInFailure.InvalidPassword);
            }

            if (await this.usersService.IsUserLocked(user.Id))
            {
                return FailedSignIn(SignInFailure.LockedOut);
            }

            await this.PublishSignInEvent(SignInType.Succeeded, user.Id, command.IpAddress,
                cancellationToken: cancellationToken);

            return new SignInValidationDto(true, userId: user.Id);
        }
        
        private static SignInValidationDto FailedSignIn(SignInFailure signInFailure)
            => new SignInValidationDto(false, signInFailure);

        private Task PublishSignInEvent(SignInType signInType, Guid userId, IPAddress ipAddress,
            string additionalInfo = null, CancellationToken cancellationToken = default)
            => this.bus.RaiseEventAsync(
                new UserSignedInEvent(signInType.ToString(), ipAddress, userId, DateTime.UtcNow, additionalInfo),
                cancellationToken);
    }
}