using System.Net.Http;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Services
{
    public class IdentityServerClient : IIdentityServerClient
    {
        public IdentityServerClient(HttpClient client) => this.Client = client;

        public HttpClient Client { get; }
    }
}