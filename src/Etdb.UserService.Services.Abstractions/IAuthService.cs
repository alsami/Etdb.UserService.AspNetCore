using System.Threading.Tasks;
using Etdb.UserService.Domain;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IAuthService
    {
        Task RegisterAsync(User user);
    }
}