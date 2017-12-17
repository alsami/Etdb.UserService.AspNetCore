using System;
using System.Collections.Generic;
using System.Security.Claims;
using Etdb.ServiceBase.Repositories.Abstractions.Generics;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Repositories.Abstractions
{
    public interface IUserRepository : IEntityRepository<User>
    {
        IEnumerable<Claim> GetClaims(User user);

        User FindWithIncludes(Guid id);

        User Find(string userName);

        User Find(string userName, string email);

        void Register(User user);
    }
}
