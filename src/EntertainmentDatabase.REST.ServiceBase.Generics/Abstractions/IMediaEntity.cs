using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EntertainmentDatabase.REST.ServiceBase.Generics.Enums;

namespace EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions
{
    public interface IMediaEntity : IEntity
    {
        string Name
        {
            get;
            set;
        }

        string Extension
        {
            get;
            set;
        }

        Guid UniqueName
        {
            get;
            set;
        }

        MediaType MediaType
        {
            get;
            set;
        }

        byte[] File
        {
            get;
            set;
        }
    }
}
