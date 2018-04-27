using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.DocumentRepository.Abstractions.Generics;
using Etdb.UserService.Domain;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface IUsersRepository : IDocumentRepository<User, Guid>
    {
        Task<IEnumerable<Claim>> AllocateClaims(User user);
        
        Task<User> FindUserAsync(Guid id);

        Task<User> FindUserAsync(string userName);

        Task<User> FindUserAsync(string userName, string emailAddress);
        
        Task RegisterAsync(User user);
    }
}