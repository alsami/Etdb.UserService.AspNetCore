using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IUserChildUrlFactory<in TUserChild> where TUserChild : UserChildDocument
    {
        string GenerateUrl(TUserChild child, string route);
    }
}