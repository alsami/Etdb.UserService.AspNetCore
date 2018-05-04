using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cqrs.Validation;
using Etdb.ServiceBase.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation
{
    public class UserRegisterCommandValidation : VoidCommandValidation<UserRegisterCommand>
    {
        private readonly IUserService userService;
        private readonly IVoidCommandValidation<EmailAddCommand> emailCommandValidation;

        public UserRegisterCommandValidation(IUserService userService, IVoidCommandValidation<EmailAddCommand> emailCommandValidation)
        {
            this.userService = userService;
            this.emailCommandValidation = emailCommandValidation;
            
            this.UserNameRules();
            this.EmailRules();
            this.PasswordRules();
        }
        
        private void UserNameRules()
        {
            this.RuleFor(command => command.UserName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Username must be given!")
                .NotEqual("Administrator", StringComparer.OrdinalIgnoreCase)
                .NotEqual("Admin", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Username blacklisted!")
                .MustAsync(async (command, userName, token) => await this.IsUserNameAvailable(userName));

        }

        private void EmailRules()
        {
            this.RuleFor(command => command.Emails)
                .Must(HasOnlyOnePrimaryEmail)
                .WithMessage("At least one email address must be given and only one can be marked as primary!");

            this.RuleForEach(command => command.Emails)
                .MustAsync(async (command, emailAddCommand, token) =>
                {
                    var result = await this.emailCommandValidation.ValidateCommandAsync(emailAddCommand);

                    if (!result.IsValid)
                    {
                        throw result.GenerateValidationException(
                            $"Error validating emailaddress {emailAddCommand.Address}");
                    }
                    
                    return true;
                })
                .MustAsync(async (command, emailCommand, token) => await this.IsEmailAbailable(emailCommand.Address))
                .WithMessage("Email address already taken!");
        }

        private void PasswordRules()
        {
            this.RuleFor(command => command.UserName)
                .NotEmpty()
                .WithMessage("A password must be given!")
                .MinimumLength(8)
                .WithMessage("A password should have a length of 8 letters!");
        }

        private static bool HasOnlyOnePrimaryEmail(ICollection<EmailAddCommand> commands)
        {
            return commands?.Count(email => email.IsPrimary) == 1;
        }

        private async Task<bool> IsUserNameAvailable(string userName)
        {
            var user = await this.userService.FindUserByUserNameAsync(userName);

            return user == null;
        }

        private async Task<bool> IsEmailAbailable(string address)
        {
            return await this.userService.IsEmAilAddressAvailable(address);
        }
    }
}