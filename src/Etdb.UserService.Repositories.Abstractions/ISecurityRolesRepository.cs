using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Generics;
using Etdb.UserService.Domain.Documents;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface ISecurityRolesRepository : IDocumentRepository<SecurityRole, Guid>
    {
    }
}