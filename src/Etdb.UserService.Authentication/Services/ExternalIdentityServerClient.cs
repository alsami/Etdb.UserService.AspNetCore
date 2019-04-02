using System.Net.Http;
using Etdb.UserService.Authentication.Abstractions.Services;

namespace Etdb.UserService.Authentication.Services
{
    public class ExternalIdentityServerClient : IExternalIdentityServerClient
    {
        public ExternalIdentityServerClient(HttpClient client) => this.Client = client;

        public HttpClient Client { get; }
    }
}