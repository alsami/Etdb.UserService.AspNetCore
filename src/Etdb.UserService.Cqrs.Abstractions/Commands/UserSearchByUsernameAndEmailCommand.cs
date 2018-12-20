using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserSearchByUsernameAndEmailCommand : IResponseCommand<UserDto>
    {
        public string UserNameOrEmail { get; }

        public UserSearchByUsernameAndEmailCommand(string userNameOrEmail)
        {
            this.UserNameOrEmail = userNameOrEmail;
        }
    }
}