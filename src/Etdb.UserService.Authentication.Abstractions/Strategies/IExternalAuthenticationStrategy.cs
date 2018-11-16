using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace Etdb.UserService.Authentication.Abstractions.Strategies
{
    public interface IExternalAuthenticationStrategy
    {
        Task<GrantValidationResult> AuthenticateAsync(string token);
    }
}