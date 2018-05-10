using System;
using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain
{
    public class SecurityRole : GuidDocument
    {
        public SecurityRole(Guid id, string name) : base(id)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}