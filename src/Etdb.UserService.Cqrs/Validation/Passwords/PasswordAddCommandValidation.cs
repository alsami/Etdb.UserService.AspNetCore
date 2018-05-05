using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Validation.Base;

namespace Etdb.UserService.Cqrs.Validation.Passwords
{
    public class PasswordAddCommandValidation : PasswordCommandValidation<PasswordAddCommand>
    {
        public PasswordAddCommandValidation()
        {
            this.RegisterPasswordRules();
        }
    }
}