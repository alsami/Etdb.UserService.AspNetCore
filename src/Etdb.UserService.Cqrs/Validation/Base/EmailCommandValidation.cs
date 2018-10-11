using Etdb.ServiceBase.Cqrs.Validation;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Base
{
    public abstract class EmailCommandValidation<TEmailCommand> : CommandValidation<TEmailCommand>
        where TEmailCommand : EmailCommand
    {
        private readonly IUsersService usersService;

        protected EmailCommandValidation(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        protected void RegisterEmailRules()
        {
            this.RuleFor(command => command.Address)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email-address must be given!");

            this.RuleFor(command => command)
                .Must(this.CheckEmailAvailibility)
                .WithMessage("Email address already taken!");
        }


        private bool CheckEmailAvailibility(EmailCommand command)
        {
            var email = this.usersService.FindEmailAddress(command.Address);

            return email == null || email.Id == command.Id;
        }
    }
}