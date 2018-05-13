using System.Threading.Tasks;
using Etdb.UserService.Domain.Documents;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUserChangesService
    {
        Task<bool> EditUserAsync(User user);
    }
}