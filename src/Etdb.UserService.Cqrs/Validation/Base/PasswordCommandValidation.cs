using Etdb.ServiceBase.Cqrs.Validation;
using Etdb.UserService.Cqrs.Abstractions.Base;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Base
{
    public abstract class PasswordCommandValidation<TPasswordCommand> : CommandValidation<TPasswordCommand> where TPasswordCommand : PasswordCommand
    {
        protected void RegisterPasswordRules()
        {
            this.RuleFor(command => command.NewPassword)
                .NotEmpty()
                .WithMessage("!")
                .MinimumLength(8)
                .WithMessage("A password must be given and should have a length of at least eight characters!");
        }
    }
}