using System.Collections.Generic;
using System.Linq;
using Etdb.UserService.Cqrs.Abstractions.Commands.Emails;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Validation.Base;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;

#nullable enable

namespace Etdb.UserService.Cqrs.Validation.Users
{
    public class UserRegisterCommandValidation : UserNameAbstractValidator<UserRegisterCommand>
    {
        public UserRegisterCommandValidation(IUsersService usersService) : base(usersService)
        {
            this.RegisterUserNameRules();
            this.EmailRules();
        }

        private void EmailRules()
        {
            this.RuleFor(command => command.Emails)
                .Must(HasOnlyOnePrimaryEmail)
                .WithMessage("At least one email address must be given and only one can be marked as primary!");
        }

        private static bool HasOnlyOnePrimaryEmail(ICollection<EmailAddCommand>? commands)
        {
            return commands?.Count(email => email.IsPrimary) == 1;
        }
    }
}