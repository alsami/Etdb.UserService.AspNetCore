using System.Threading.Tasks;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions.Models;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IProfileImageStorageService
    {
        Task StoreAsync(StorableImage storableImage);

        Task RemoveAsync(ProfileImage profileImage);
    }
}