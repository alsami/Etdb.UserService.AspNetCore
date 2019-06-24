using System;

namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class UserNameCommand
    {
        public Guid Id { get; }

        public string WantedUserName { get; }

        protected UserNameCommand(Guid id, string wantedUserName)
        {
            this.Id = id;
            this.WantedUserName = wantedUserName;
        }
    }
}