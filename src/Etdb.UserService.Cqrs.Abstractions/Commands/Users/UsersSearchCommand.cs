using System.Collections.Generic;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UsersSearchCommand : IResponseCommand<IEnumerable<UserFlatDto>>
    {
        public string SearchTerm { get; }

        public UsersSearchCommand(string searchTerm)
        {
            this.SearchTerm = searchTerm;
        }
    }
}