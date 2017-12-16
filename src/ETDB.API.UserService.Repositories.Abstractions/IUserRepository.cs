using System.Collections.Generic;
using System.Security.Claims;
using Etdb.UserService.Domain.Entities;
using ETDB.API.ServiceBase.Repositories.Abstractions.Generics;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface IUserRepository : IEntityRepository<User>
    {
        IEnumerable<Claim> GetClaims(User user);

        User Find(string userName);

        User Find(string userName, string email);

        void Register(User user);
    }
}
