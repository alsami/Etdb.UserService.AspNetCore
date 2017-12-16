using Etdb.UserService.EventSourcing.Commands;
using Etdb.UserService.Repositories.Abstractions;

namespace Etdb.UserService.EventSourcing.Validation
{
    public abstract class UserCommandValidation : UserValidation<UserCommand>
    {
        protected UserCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.ValidateUserName();
            this.ValidateName();
            this.ValidateLastName();
            this.ValidateEmail();
            this.ValidatePassword();
        }
    }
}
