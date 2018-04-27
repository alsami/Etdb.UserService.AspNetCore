using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Context;
using Etdb.ServiceBase.DocumentRepository.Generics;
using Etdb.UserService.Domain;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories.Context
{
    public class SecurityRolesRepository : GenericDocumentRepository<SecurityRole, Guid>, ISecurityRolesRepository
    {
        public SecurityRolesRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}