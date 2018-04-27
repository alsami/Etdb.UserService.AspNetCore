using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cqrs.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Extensions;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserRegisterCommandHandler : ResponseCommandHandler<UserRegisterCommand, UserDto>
    {
        public UserRegisterCommandHandler(IResponseCommandValidation<UserRegisterCommand, UserDto> commandValidation) : base(commandValidation)
        {
        }

        public override async Task<UserDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var validation = await this.CommandValidation.ValidateCommandAsync(request);

            if (!validation.IsValid)
            {
                validation.ThrowValidationError("Error validating user registration!");
            }

            return null;
        }
    }
}