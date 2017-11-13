using System;
using System.Collections.Generic;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.UserService.Domain.Entities
{
    public class Securityrole : IEntity
    {
        public Securityrole()
        {
            this.UserSecurityroles = new List<UserSecurityrole>();
        }

        public Guid Id
        {
            get;
            set;
        }

        public byte[] RowVersion
        {
            get;
            set;
        }

        public string Designation
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public ICollection<UserSecurityrole> UserSecurityroles
        {
            get;
            set;
        }
    }
}
