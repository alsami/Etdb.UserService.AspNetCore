using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Abstractions.Models;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserProfileImageLoadCommand : IResponseCommand<FileInfo>
    {
        public UserProfileImageLoadCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}