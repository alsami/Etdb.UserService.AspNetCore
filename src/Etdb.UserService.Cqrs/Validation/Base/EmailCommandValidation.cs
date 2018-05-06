using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Validation;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Base
{
    public abstract class EmailCommandValidation<TEmailCommand> : CommandValidation<TEmailCommand> where TEmailCommand : EmailCommand
    {
        private readonly IUsersSearchService usersSearchService;

        protected EmailCommandValidation(IUsersSearchService usersSearchService)
        {
            this.usersSearchService = usersSearchService;
        }
        
        protected void RegisterEmailRules()
        {
            this.RuleFor(command => command.Address)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email-address must be given!");
            
            this.RuleFor(command => command)
                .MustAsync(async (command, token, email) => await this.IsEmailAbailable(command))
                .WithMessage("Email address already taken!");
        }
        
        
        private async Task<bool> IsEmailAbailable(EmailCommand command)
        {
            var email = await this.usersSearchService.FindEmailAddress(command.Address);

            if (email == null)
            {
                return true;
            }

            return email.Id == command.Id;
        }
    }
}