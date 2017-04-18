using EntertainmentDatabase.REST.API.Domain.Enums;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.Domain.Base
{
    public interface IMediaFile : IEntity
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

        MediaFileType MediaFileType
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
