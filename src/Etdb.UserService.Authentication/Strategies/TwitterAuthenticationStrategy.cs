using System;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using IdentityServer4.Validation;

namespace Etdb.UserService.Authentication.Strategies
{
    public class TwitterAuthenticationStrategy : ExternalAuthenticationStrategyBase, ITwitterAuthenticationStrategy
    {
        public Task<GrantValidationResult> AuthenticateAsync(string token)
        {
            throw new System.NotImplementedException();
        }

        protected override string UserProfileUrl => throw new NotImplementedException();
    }
}