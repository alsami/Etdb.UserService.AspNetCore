using System.Linq;
using Etdb.ServiceBase.ErrorHandling.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Etdb.UserService.Controllers.Extensions
{
    internal static class ModelStateExtensions
    {
        public static void ThrowValidationError(this ModelStateDictionary modelState, string message)
        {
            var errors = modelState.Values.SelectMany(error => error.Errors)
                .Select(error => error.ErrorMessage).ToArray();

            errors = errors.Length == 0 ? new[] {"Object could not be casted"} : errors;
            
            throw new GeneralValidationException(message, errors);
        }
    }
}