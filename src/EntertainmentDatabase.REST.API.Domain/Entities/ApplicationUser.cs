using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Domain.Entities
{
    public class ApplicationUser : IEntity
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

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
