using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Exceptions;
using Etdb.ServiceBase.Extensions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class UserPasswordChangeCommandHandler : IRequestHandler<UserPasswordChangeCommand>
    {
        private readonly AbstractValidator<UserPasswordChangeCommand> passwordChangeCommandValidation;
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IHasher hasher;

        public UserPasswordChangeCommandHandler(
            AbstractValidator<UserPasswordChangeCommand> passwordChangeCommandValidation, IUsersService usersService,
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
                throw WellknownExceptions.UserResourceLockException(existingUser.Id);
            }

            var validationResult = await this.passwordChangeCommandValidation.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);

                throw validationResult.GenerateValidationException(
                    "Failed to change the password.");
            }

            var salt = this.hasher.GenerateSalt();

            var updatedUser =
                existingUser.MutateCredentials(this.hasher.CreateSaltedHash(command.NewPassword, salt), salt);

            await this.usersService.EditAsync(updatedUser);

            await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);

            return Unit.Value;
        }
    }
}