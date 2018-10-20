using System.Reflection;
using Etdb.ServiceBase.Cqrs.Validation;
using Etdb.UserService.Cqrs.Abstractions.Base;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Base
{
    public abstract class PasswordCommandValidation<TPasswordCommand> : CommandValidation<TPasswordCommand>
        where TPasswordCommand : PasswordCommand
    {
        protected virtual string PasswordTooShortMessage =>
            "{0} must have a length of at least eight characters!;";

        protected virtual int PasswordMinLength => 8;

        protected void RegisterDefaultPasswordRule(string passwordFieldName)
        {
            this.RuleFor(command => command.NewPassword)
                .MinimumLength(this.PasswordMinLength)
                .WithMessage(string.Format(this.PasswordTooShortMessage, passwordFieldName));
        }
    }
}