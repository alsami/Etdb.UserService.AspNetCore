using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.ServiceBase.Repositories.Abstractions.Generics;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<IEnumerable<Claim>> GetClaims(User user);

        Task<User> FindAsync(string userName);

        Task<User> FindAsync(string userName, string email);

        Task RegisterAsync(User user);
    }
}
