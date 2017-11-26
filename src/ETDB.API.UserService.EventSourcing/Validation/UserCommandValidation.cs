using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.Repositories.Repositories;

namespace ETDB.API.UserService.EventSourcing.Validation
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
