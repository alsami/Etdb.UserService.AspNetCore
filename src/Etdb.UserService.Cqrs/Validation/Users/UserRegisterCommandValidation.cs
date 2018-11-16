using System.Collections.Generic;
using System.Linq;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Validation.Base;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Users
{
    public class UserRegisterCommandValidation : UserNameCommandValidation<UserRegisterCommand>
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

        private static bool HasOnlyOnePrimaryEmail(ICollection<EmailAddCommand> commands)
        {
            return commands?.Count(email => email.IsPrimary) == 1;
        }
    }
}