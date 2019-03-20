using System.Net;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserSigninValidationCommand : IResponseCommand<UserLoginValidationDto>
    {
        public string UserName { get; }

        public string Password { get; }

        public IPAddress IpAddress { get; }

        public UserSigninValidationCommand(string userName, string password, IPAddress ipAddress)
        {
            this.UserName = userName;
            this.Password = password;
            this.IpAddress = ipAddress;
        }
    }
}