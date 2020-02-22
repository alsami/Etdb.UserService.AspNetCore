using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUserUrlFactory
    {
        string GenerateUrl(User user, string route);
    }
}