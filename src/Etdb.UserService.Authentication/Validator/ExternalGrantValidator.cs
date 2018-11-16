using System;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Constants;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Etdb.UserService.Authentication.Validator
{
    public class ExternalGrantValidator : IExtensionGrantValidator
    {
        private const string ProviderKey = "Provider";
        private const string UnsupportedProviderMessage = "Loginprovider not supported!";
        private readonly Func<SignInProvider, IExternalAuthenticationStrategy> loginStrategyComposer;

        public ExternalGrantValidator(Func<SignInProvider, IExternalAuthenticationStrategy> loginStrategyComposer)
        {
            this.loginStrategyComposer = loginStrategyComposer;
        }

        public string GrantType => Misc.ExternalGrantType;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            if (!Enum.TryParse<SignInProvider>(context.Request.Raw.Get("Provider"), true, out var loginProvider) ||
                loginProvider == SignInProvider.UsernamePassword)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                    ExternalGrantValidator.UnsupportedProviderMessage);
                return;
            }

            var strategy = this.loginStrategyComposer(loginProvider);

            context.Result = await strategy.AuthenticateAsync(context.Request.Raw.Get("Token"));
        }
    }
}