using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Bus;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Validation;

namespace Etdb.UserService.Authentication.Strategies
{
    public class TwitterAuthenticationStrategy : ExternalAuthenticationStrategyBase, ITwitterAuthenticationStrategy
    {
        public TwitterAuthenticationStrategy(IBus bus, IExternalIdentityServerClient externalIdentityServerClient) :
            base(bus, externalIdentityServerClient)
        {
        }

        protected override string UserProfileUrl => "https://api.twitter.com/1.1/account/verify_credentials.json";
        protected override AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Twitter;

        public Task<GrantValidationResult> AuthenticateAsync(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}