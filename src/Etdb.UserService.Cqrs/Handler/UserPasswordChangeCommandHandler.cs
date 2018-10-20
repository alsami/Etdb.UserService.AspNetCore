using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Cqrs.Abstractions.Validation;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Exceptions;
using Etdb.ServiceBase.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Documents;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserPasswordChangeCommandHandler : IVoidCommandHandler<UserPasswordChangeCommand>
    {
        private readonly IHasher hasher;
        private readonly ICommandValidation<UserPasswordChangeCommand> passwordChangeCommandValidation;
        private readonly IUsersService usersService;

        public UserPasswordChangeCommandHandler(
            ICommandValidation<UserPasswordChangeCommand> passwordChangeCommandValidation, IUsersService usersService,
            IHasher hasher)
        {
            this.passwordChangeCommandValidation = passwordChangeCommandValidation;
            this.usersService = usersService;
            this.hasher = hasher;
        }

        public async Task<Unit> Handle(UserPasswordChangeCommand command, CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(command.Id);

            if (user == null) throw new ResourceNotFoundException("User could not be found!");

            var validationResult = await this.passwordChangeCommandValidation.ValidateCommandAsync(command);

            if (!validationResult.IsValid)
                validationResult.GenerateValidationException(
                    "Failed to validate the request to change the user's password!");

            var salt = this.hasher.GenerateSalt();

            var updatedUser = new User(user.Id, user.UserName, user.FirstName, user.Name, user.Biography,
                this.hasher.CreateSaltedHash(command.NewPassword, salt), salt, user.RegisteredSince, user.ProfileImage,
                user.RoleIds, user.Emails.Select(email => email.Clone()).ToArray());

            await this.usersService.EditAsync(updatedUser);

            return Unit.Value;
        }
    }
}