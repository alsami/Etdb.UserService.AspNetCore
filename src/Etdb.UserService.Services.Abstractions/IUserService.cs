using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.UserService.Domain;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUserService
    {        
        Task<IEnumerable<Claim>> AllocateClaims(User user);
        
        Task<User> FindUserByIdAsync(Guid id);

        Task<User> FindUserByUserNameAsync(string userName);

        Task<User> FindUserByUserNameOrEmailAsync(string userNameOrEmail);
        
        Task RegisterAsync(User user);

        Task<bool> IsEmAilAddressAvailable(string emailAddress);

        bool ArePasswordsEqual(User user, string password);
    }
}