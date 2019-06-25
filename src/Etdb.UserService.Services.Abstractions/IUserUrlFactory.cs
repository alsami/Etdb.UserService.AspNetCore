using Etdb.UserService.Domain.Base;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUserUrlFactory
    {
        string GenerateUrlWithChildIdParameter<TUserChild>(TUserChild child, string route) where TUserChild : UserChildDocument;

        string GenerateUrl(User user, string route);
    }
}