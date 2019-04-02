using System.Net;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Authentication;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserSignInValidationCommand : IResponseCommand<SignInValidationDto>
    {
        public string UserName { get; }

        public string Password { get; }

        public IPAddress IpAddress { get; }

        public UserSignInValidationCommand(string userName, string password, IPAddress ipAddress)
        {
            this.UserName = userName;
            this.Password = password;
            this.IpAddress = ipAddress;
        }
    }
}