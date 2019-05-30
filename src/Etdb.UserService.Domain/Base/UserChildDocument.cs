using System;

namespace Etdb.UserService.Domain.Base
{
    public abstract class UserChildDocument : GuidDocument
    {
        
        protected UserChildDocument(Guid id, Guid userId) : base(id)
        {
            this.UserId = userId;
        }
        
        public Guid UserId { get; protected set; }
    }
}