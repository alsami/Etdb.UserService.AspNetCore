using System.Net;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Abstractions.Services;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Validation;
using MediatR;

namespace Etdb.UserService.Authentication.Strategies
{
    public class TwitterAuthenticationStrategy : ExternalAuthenticationStrategyBase, ITwitterAuthenticationStrategy
    {
        private string UserProfileUrl => "https://api.twitter.com/1.1/account/verify_credentials.json";

        public TwitterAuthenticationStrategy(IMediator bus, IExternalIdentityServerClient externalIdentityServerClient)
            : base(bus, externalIdentityServerClient)
        {
        }


        // ReSharper disable once UnusedMember.Local
        protected override AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Twitter;

        public Task<GrantValidationResult> AuthenticateAsync(IPAddress ipAddress, string token) =>
            throw new System.NotImplementedException();
    }
}