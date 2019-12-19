using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Validation.Base;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Cqrs.Validation.Users
{
    public class UserNameChangeCommandValidation : UserNameAbstractValidator<UserNameChangeCommand>
    {
        public UserNameChangeCommandValidation(IUsersService usersService) : base(usersService)
        {
            this.RegisterUserNameRules();
        }
    }
}