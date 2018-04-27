using System.Linq;
using Etdb.ServiceBase.ErrorHandling.Abstractions.Exceptions;
using FluentValidation.Results;

namespace Etdb.UserService.Cqrs.Extensions
{
    internal static class ValidationResultExtensions
    {
        public static void ThrowValidationError(this ValidationResult result, string primaryMessage)
        {
            throw new GeneralValidationException(primaryMessage,
                result.Errors.Select(error => error.ErrorMessage).ToArray());
        }
    }
}