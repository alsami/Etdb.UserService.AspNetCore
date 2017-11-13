using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Base
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
