using System.Net;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserAuthenticationValidationCommand : IResponseCommand<AuthenticationValidationDto>
    {
        public string UserName { get; }

        public string Password { get; }

        public IPAddress IpAddress { get; }

        public UserAuthenticationValidationCommand(string userName, string password, IPAddress ipAddress)
        {
            this.UserName = userName;
            this.Password = password;
            this.IpAddress = ipAddress;
        }
    }
}