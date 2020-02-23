using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Repositories.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Base
{
    public abstract class EmailAbstractValidator<TEmailCommand> : AbstractValidator<TEmailCommand>
        where TEmailCommand : EmailCommand
    {
        private readonly IUsersRepository usersRepository;

        protected EmailAbstractValidator(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
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
            var email = this.usersRepository.FindEmailAddress(command.Address);

            return email is null || email.Id == command.Id;
        }
    }
}