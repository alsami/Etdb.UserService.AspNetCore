using System;
using Etdb.UserService.Presentation.DTO.Base;

namespace Etdb.UserService.Presentation.DTO
{
    public class SecurityRoleDTO : IDataTransferObject
    {
        public Guid Id { get; set; }
        public byte[] ConccurencyToken { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }
    }
}
