using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstractions;

namespace EntertainmentDatabase.REST.API.Entities
{
    public class Test : IEntity
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
    }
}
