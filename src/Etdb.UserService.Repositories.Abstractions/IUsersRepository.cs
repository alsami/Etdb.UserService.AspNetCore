using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Generics;
using Etdb.UserService.Domain.Documents;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface IUsersRepository : IDocumentRepository<User, Guid>
    {
    }
}