using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain;

namespace Etdb.UserService.Repositories
{
    public class SecurityRoleRepository : GenericDocumentRepository<SecurityRole, Guid>
    {
        public SecurityRoleRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}