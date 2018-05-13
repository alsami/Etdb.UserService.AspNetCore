using System;
using Etdb.ServiceBase.Cqrs.Abstractions.Commands;

namespace Etdb.UserService.Cqrs.Abstractions.Commands
{
    public class UserProfileImageBytesLoadCommand : IResponseCommand<byte[]>
    {
        public UserProfileImageBytesLoadCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}