using ETDB.API.UserService.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace ETDB.API.UserService.Repositories.Base
{
    public interface IUserClaimsRepository
    {
        IEnumerable<Claim> GetClaims(User user);
    }
}
