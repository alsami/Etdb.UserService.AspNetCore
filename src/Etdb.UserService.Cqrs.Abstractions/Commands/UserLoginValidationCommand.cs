using System.Net;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserLoginValidationCommand : IResponseCommand<UserLoginValidationDto>
    {
        public string UserName { get; }

        public string Password { get; }

        public IPAddress IpAddress { get; }

        public UserLoginValidationCommand(string userName, string password, IPAddress ipAddress)
        {
            this.UserName = userName;
            this.Password = password;
            this.IpAddress = ipAddress;
        }
    }
}