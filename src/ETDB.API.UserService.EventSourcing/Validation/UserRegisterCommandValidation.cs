using ETDB.API.UserService.EventSourcing.Commands;
using ETDB.API.UserService.Repositories.Repositories;
using FluentValidation.Results;

namespace ETDB.API.UserService.EventSourcing.Validation
{
    public class UserRegisterCommandValidation : UserCommandValidation
    {
        public UserRegisterCommandValidation(IUserRepository userRepository) : base(userRepository)
        {
            this.ValidateUserNameAndEmailNotTaken();
        }

        public override bool IsValid(UserCommand sourcingCommand, out ValidationResult validationResult)
        {
            validationResult = this.Validate(sourcingCommand);
            return validationResult.IsValid;
        }
    }
}
