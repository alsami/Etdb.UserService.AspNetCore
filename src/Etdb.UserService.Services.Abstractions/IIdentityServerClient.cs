using System.Net.Http;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IIdentityServerClient
    {
        HttpClient Client { get; }
    }
}
