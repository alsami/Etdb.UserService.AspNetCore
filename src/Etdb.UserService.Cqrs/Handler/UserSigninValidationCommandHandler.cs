using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserSigninValidationCommandHandler :
        IResponseCommandHandler<UserSigninValidationCommand, UserLoginValidationDto>
    {
        private readonly IUsersService usersService;
        private readonly ILogger<UserSigninValidationCommandHandler> logger;
        private readonly IHasher hasher;
        private readonly ILoginLogRepository loginLogRepository;
        private static readonly UserLoginValidationDto FailedLogin = new UserLoginValidationDto(false);

        public UserSigninValidationCommandHandler(IUsersService usersService, IHasher hasher,
            ILoginLogRepository loginLogRepository, ILogger<UserSigninValidationCommandHandler> logger)
        {
            this.usersService = usersService;
            this.hasher = hasher;
            this.loginLogRepository = loginLogRepository;
            this.logger = logger;
        }

        public async Task<UserLoginValidationDto> Handle(UserSigninValidationCommand command,
            CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByUserNameOrEmailAsync(command.UserName);

            if (user == null)
            {
                return UserSigninValidationCommandHandler.FailedLogin;
            }

            var passwordIsValid = this.hasher.CreateSaltedHash(command.Password, user.Salt)
                                  == user.Password;

            if (passwordIsValid)
            {
                return new UserLoginValidationDto(true, user.Id);
            }

            await this.LogLoginEvent(LoginType.Failed, user.Id, command.IpAddress, "Given password is invalid!");

            return UserSigninValidationCommandHandler.FailedLogin;
        }

        private async Task LogLoginEvent(LoginType loginType, Guid userId, IPAddress ipAddress,
            string additionalInfo = null)
        {
            var log = new LoginLog(Guid.NewGuid(), userId, DateTime.UtcNow, loginType,
                ipAddress.ToString(), additionalInfo);

            try
            {
                await this.loginLogRepository.AddAsync(log);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, ex.Message);
            }
        }
    }
}