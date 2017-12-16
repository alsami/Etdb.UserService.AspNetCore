using System;

namespace Etdb.UserService.Presentation.DTO.Base
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
