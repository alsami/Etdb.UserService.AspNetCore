using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.API.Abstraction;
using EntertainmentDatabase.REST.API.Abstraction.Base;

namespace EntertainmentDatabase.REST.API.DataTransferObjects
{
    public class MovieDTO : IDataTransferObject
    {
        public byte[] ConcurrencyToken
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }
    }
}
    