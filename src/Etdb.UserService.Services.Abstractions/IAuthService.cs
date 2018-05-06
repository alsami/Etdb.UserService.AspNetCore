using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Etdb.UserService.Domain;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IAuthService
    {
        Task RegisterAsync(User user);
    }
}