using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.Domain.Abstraction
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
