using System;
using System.Collections.Generic;
using System.Text;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.Domain.Base
{
    public interface ILogInfo : IEntity
    {
        string TraceId
        {
            get;
            set;
        }

        string HttpMethod
        {
            get;
            set;
        }

        string Path
        {
            get;
            set;
        }
    }
}
