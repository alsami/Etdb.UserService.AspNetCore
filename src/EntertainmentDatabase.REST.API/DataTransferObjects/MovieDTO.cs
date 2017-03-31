using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.API.Abstraction;

namespace EntertainmentDatabase.REST.API.DataTransferObjects
{
    public class MovieDTO : IDTO
    {
        public Guid Id { get; set; }
    }
}
