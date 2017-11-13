using System;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.UserService.Domain.Entities
{
    public class UserSecurityrole : IEntity
    {
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

        public User User
        {
            get;
            set;
        }

        public Guid UserId
        {
            get;
            set;
        }

        public Securityrole Securityrole
        {
            get;
            set;
        }

        public Guid SecurityroleId
        {
            get;
            set;
        }
    }
}
