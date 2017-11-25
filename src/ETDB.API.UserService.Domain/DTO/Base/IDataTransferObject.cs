using System;

namespace ETDB.API.UserService.Domain.DTO.Base
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
