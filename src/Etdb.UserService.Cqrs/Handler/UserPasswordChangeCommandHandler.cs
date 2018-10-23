using System;
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
        private readonly ICommandValidation<UserPasswordChangeCommand> passwordChangeCommandValidation;
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IHasher hasher;

        public UserPasswordChangeCommandHandler(
            ICommandValidation<UserPasswordChangeCommand> passwordChangeCommandValidation, IUsersService usersService,
            IHasher hasher, IResourceLockingAdapter resourceLockingAdapter)
        {
            this.passwordChangeCommandValidation = passwordChangeCommandValidation;
            this.usersService = usersService;
            this.hasher = hasher;
            this.resourceLockingAdapter = resourceLockingAdapter;
        }

        public async Task<Unit> Handle(UserPasswordChangeCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByIdAsync(command.Id);

            if (existingUser == null)
            {
                throw new ResourceNotFoundException("User could not be found!");
            }

            if (!await this.resourceLockingAdapter.LockAsync(existingUser.Id, TimeSpan.FromSeconds(30)))
            {
                throw new ResourceLockedException(typeof(User), existingUser.Id, "User currently busy");
            }

            var validationResult = await this.passwordChangeCommandValidation.ValidateCommandAsync(command);

            if (!validationResult.IsValid)
            {
                throw validationResult.GenerateValidationException(
                    "Failed to validate the request to change the user's password!");
            }

            var salt = this.hasher.GenerateSalt();

            var updatedUser = new User(existingUser.Id, existingUser.UserName, existingUser.FirstName,
                existingUser.Name, existingUser.Biography,
                this.hasher.CreateSaltedHash(command.NewPassword, salt), salt, existingUser.RegisteredSince,
                existingUser.ProfileImage,
                existingUser.RoleIds, existingUser.Emails);

            await this.usersService.EditAsync(updatedUser);

            await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);

            return Unit.Value;
        }
    }
}