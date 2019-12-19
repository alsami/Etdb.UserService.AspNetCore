using Etdb.UserService.Presentation.Users;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserNameAvailabilityCheckCommand : IRequest<UserNameAvailabilityDto>
    {
        public string UserName { get; }

        public UserNameAvailabilityCheckCommand(string userName)
        {
            this.UserName = userName;
        }
    }
}