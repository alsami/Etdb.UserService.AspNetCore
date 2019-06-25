using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;

namespace Etdb.UserService.Authentication.Strategies
{
    public class TwitterAuthenticationStrategy : ExternalAuthenticationStrategyBase, ITwitterAuthenticationStrategy
    {
        private string UserProfileUrl => "https://api.twitter.com/1.1/account/verify_credentials.json";

        public TwitterAuthenticationStrategy(IBus bus, IExternalIdentityServerClient externalIdentityServerClient,
            IHttpContextAccessor httpContextAccessor) : base(bus, externalIdentityServerClient, httpContextAccessor)
        {
        }


        // ReSharper disable once UnusedMember.Local
        protected override AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Twitter;

        public Task<GrantValidationResult> AuthenticateAsync(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}