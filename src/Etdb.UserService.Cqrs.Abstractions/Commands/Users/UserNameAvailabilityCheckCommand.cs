using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UserNameAvailabilityCheckCommand : IResponseCommand<UserNameAvailabilityDto>
    {
        public string UserName { get; }

        public UserNameAvailabilityCheckCommand(string userName)
        {
            this.UserName = userName;
        }
    }
}