using System;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.Repositories
{
    public class SecurityRolesRepository : GenericDocumentRepository<SecurityRole, Guid>, ISecurityRolesRepository
    {
        public SecurityRolesRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}