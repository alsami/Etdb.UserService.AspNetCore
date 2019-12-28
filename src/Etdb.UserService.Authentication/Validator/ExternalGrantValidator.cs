using System;
using System.Net;
using System.Threading.Tasks;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Domain.Enums;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Etdb.UserService.Authentication.Validator
{
    public class ExternalGrantValidator : IExtensionGrantValidator
    {
        private const string UnsupportedProviderMessage = "Loginprovider not supported!";
        private readonly Func<AuthenticationProvider, IExternalAuthenticationStrategy> loginStrategyComposer;

        public ExternalGrantValidator(
            Func<AuthenticationProvider, IExternalAuthenticationStrategy> loginStrategyComposer)
        {
            this.loginStrategyComposer = loginStrategyComposer;
        }

        public string GrantType => Misc.Constants.Identity.ExternalGrantType;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            if (!Enum.TryParse<AuthenticationProvider>(context.Request.Raw.Get("Provider"), true,
                    out var loginProvider) ||
                loginProvider == AuthenticationProvider.UsernamePassword)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                    ExternalGrantValidator.UnsupportedProviderMessage);
                return;
            }

            var strategy = this.loginStrategyComposer(loginProvider);

            var token = context.Request.Raw.Get("Token");

            var ipAddress = IPAddress.Parse(context.Request.Raw.Get("IpAddress"));

            context.Result = await strategy.AuthenticateAsync(ipAddress, token);
        }
    }
}