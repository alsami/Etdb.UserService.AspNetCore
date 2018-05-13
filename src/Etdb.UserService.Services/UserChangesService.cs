using System.Threading.Tasks;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Extensions;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Etdb.UserService.Services
{
    public class UserChangesService : IUserChangesService
    {
        private readonly IDistributedCache cache;
        private readonly IUsersRepository usersRepository;

        public UserChangesService(IUsersRepository usersRepository, IDistributedCache cache)
        {
            this.usersRepository = usersRepository;
            this.cache = cache;
        }

        public async Task<bool> EditUserAsync(User user)
        {
            var saved = await this.usersRepository.EditAsync(user);

            if (!saved)
            {
                return false;
            }
            
            await this.cache.AddOrUpdateAsync(user.Id, user);
            return true;
        }
    }
}