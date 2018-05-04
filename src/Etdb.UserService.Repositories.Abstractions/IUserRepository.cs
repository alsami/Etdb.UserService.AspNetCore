using System;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Generics;
using Etdb.UserService.Domain;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface IUserRepository : IDocumentRepository<User, Guid>
    {
    }
}