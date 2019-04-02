using System;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class UserNameCommand
    {
        public Guid Id { get; }

        public string UserName { get; }

        protected UserNameCommand(Guid id, string userName)
        {
            this.Id = id;
            this.UserName = userName;
        }
    }
}