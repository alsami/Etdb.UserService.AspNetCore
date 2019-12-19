using System.Net;
using Etdb.UserService.Presentation.Authentication;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserAuthenticationValidationCommand : IRequest<AuthenticationValidationDto>
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