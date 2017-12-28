using System;

namespace Etdb.UserService.Presentation.DataTransferObjects.Base
{
    public interface IDataTransferObject
    {
        Guid Id { get; set; }

        byte[] ConccurencyToken { get; set; }
    }
}
