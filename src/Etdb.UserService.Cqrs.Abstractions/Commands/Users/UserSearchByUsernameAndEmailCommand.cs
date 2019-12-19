using Etdb.UserService.Presentation.Users;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserSearchByUsernameAndEmailCommand : IRequest<UserDto>
    {
        public string UserNameOrEmail { get; }

        public UserSearchByUsernameAndEmailCommand(string userNameOrEmail)
        {
            this.UserNameOrEmail = userNameOrEmail;
        }
    }
}