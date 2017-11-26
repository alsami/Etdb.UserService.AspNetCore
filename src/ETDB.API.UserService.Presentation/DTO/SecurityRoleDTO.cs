using System;
using ETDB.API.UserService.Presentation.DTO.Base;

namespace ETDB.API.UserService.Presentation.DTO
{
    public class SecurityRoleDTO : IDataTransferObject
    {
        public Guid Id { get; set; }
        public byte[] ConccurencyToken { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }
    }
}
