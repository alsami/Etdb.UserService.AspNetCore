using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using EntertainmentDatabase.REST.API.Domain.Base;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.Domain.Entities
{
    public class ErrorLog : ILogInfo
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

        public string TraceId
        {
            get;
            set;
        }

        public string HttpMethod
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public DateTime Occurrence
        {
            get;
            set;
        }
    }
}
