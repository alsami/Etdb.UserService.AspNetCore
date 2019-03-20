using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Validation.Base;

namespace Etdb.UserService.Cqrs.Validation.Passwords
{
    public class PasswordAddCommandValidation : PasswordCommandValidation<PasswordAddCommand>
    {
        public PasswordAddCommandValidation()
        {
            this.RegisterDefaultPasswordRule("Password");
        }
    }
}