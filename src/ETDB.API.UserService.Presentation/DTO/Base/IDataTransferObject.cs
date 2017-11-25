using System;

namespace ETDB.API.UserService.Presentation.DTO.Base
{
    public interface IDataTransferObject
    {
        Guid Id
        {
            get;
            set;
        }

        byte[] ConccurencyToken
        {
            get;
            set;
        }
    }
}
