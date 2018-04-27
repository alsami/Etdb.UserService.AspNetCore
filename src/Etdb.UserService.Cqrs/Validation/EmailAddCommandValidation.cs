using Etdb.ServiceBase.Cqrs.Validation;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Repositories.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation
{
    public class EmailAddCommandValidation : VoidCommandValidation<EmailAddCommand>
    {

        public EmailAddCommandValidation()
        {
            this.EmailRule();
        }

        private void EmailRule()
        {
            this.RuleFor(command => command.Address)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email-address must be given!");
        }
    }
}