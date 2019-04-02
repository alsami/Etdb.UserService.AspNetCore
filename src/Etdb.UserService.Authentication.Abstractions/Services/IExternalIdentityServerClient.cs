using System.Net.Http;

namespace Etdb.UserService.Authentication.Abstractions.Services
{
    public interface IExternalIdentityServerClient
    {
        HttpClient Client { get; }
    }
}
