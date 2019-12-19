using System.Collections.Generic;
using Etdb.UserService.Presentation.Users;
using MediatR;

namespace Etdb.UserService.Cqrs.Abstractions.Commands.Users
{
    public class UsersSearchCommand : IRequest<IEnumerable<UserFlatDto>>
    {
        public string SearchTerm { get; }

        public UsersSearchCommand(string searchTerm)
        {
            this.SearchTerm = searchTerm;
        }
    }
}