using System.Collections.Generic;
using System.Security.Claims;
using ETDB.API.UserService.Domain.Entities;

namespace ETDB.API.UserService.Domain
{
    public interface IUserRepository
    {
        IEnumerable<Claim> GetClaims(User user);

        User Get(string userName);

        User Get(string userName, string email);

        void Register(User user);
    }
}
