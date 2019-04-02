using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
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