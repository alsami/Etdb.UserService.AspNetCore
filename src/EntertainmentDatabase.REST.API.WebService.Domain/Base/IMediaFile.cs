using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Enums;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Base
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
