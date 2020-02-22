using Etdb.UserService.Cqrs.Abstractions.Commands.Emails;
using Etdb.UserService.Cqrs.Validation.Base;
using Etdb.UserService.Repositories.Abstractions;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.Validation.Emails
{
    public class EmailAddCommandValidation : EmailAbstractValidator<EmailAddCommand>
    {
        public EmailAddCommandValidation(IUsersRepository usersRepository) : base(usersRepository)
        {
            this.RegisterEmailRules();
        }
    }
}