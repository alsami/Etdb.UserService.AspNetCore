using System;
using Etdb.UserService.Domain.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Etdb.UserService.Domain.Entities
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