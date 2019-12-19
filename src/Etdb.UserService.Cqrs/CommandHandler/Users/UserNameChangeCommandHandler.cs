using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Exceptions;
using Etdb.UserService.Cqrs.Abstractions.Base;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class UserNameChangeCommandHandler : IRequestHandler<UserNameChangeCommand>
    {
        private readonly AbstractValidator<UserNameChangeCommand> commandValidation;
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;

        public UserNameChangeCommandHandler(AbstractValidator<UserNameChangeCommand> commandValidation,
            IUsersService usersService, IResourceLockingAdapter resourceLockingAdapter)
        {
            this.commandValidation = commandValidation;
            this.usersService = usersService;
            this.resourceLockingAdapter = resourceLockingAdapter;
        }

        public async Task<Unit> Handle(UserNameChangeCommand command, CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(command.Id);

            if (user == null) throw WellknownExceptions.UserNotFoundException();

            await this.ValidateCommandAndThrowOnFailureAsync(command);

            var userNameResourceLockKey = $"username_change_{command.WantedUserName}";

            await this.LockResourcesAndThrowOnFailureAsync(command, userNameResourceLockKey, cancellationToken);

            var replacedUser = user.MutateUserName(command.WantedUserName);

            await this.usersService.EditAsync(replacedUser);

            await this.UnlockResourcesAsync(command, userNameResourceLockKey, cancellationToken);

            return Unit.Value;
        }

        private async Task ValidateCommandAndThrowOnFailureAsync(UserNameChangeCommand command)
        {
            var validationResult = await this.commandValidation.ValidateAsync(command);

            if (validationResult.IsValid) return;

            var errors = validationResult
                .Errors
                .Select(error => error.ErrorMessage)
                .ToArray();

            throw new GeneralValidationException("Error validating user-name change request!", errors);
        }

        private async Task LockResourcesAndThrowOnFailureAsync(UserNameCommand command, string userNameResourceLockKey,
            CancellationToken cancellationToken)
        {
            var lockingTasks = new[]
            {
                Task.Run(async () =>
                {
                    if (await this.resourceLockingAdapter.LockAsync(userNameResourceLockKey,
                        TimeSpan.FromMinutes(1))) return;

                    throw new ResourceLockedException(typeof(User), "userName",
                        "The user-name is currently busy and cannot be reserved! Please try again later");
                }, cancellationToken),
                Task.Run(async () =>
                {
                    if (await this.resourceLockingAdapter.LockAsync(command.Id, TimeSpan.FromMinutes(1))) return;

                    throw WellknownExceptions.UserResourceLockException(command.Id);
                }, cancellationToken)
            };

            await Task.WhenAll(lockingTasks);
        }

        private async Task UnlockResourcesAsync(UserNameCommand command, string userNameResourceLockKey,
            CancellationToken cancellationToken)
        {
            var unLockingTasks = new[]
            {
                Task.Run(async () => await this.resourceLockingAdapter.UnlockAsync(userNameResourceLockKey),
                    cancellationToken),
                Task.Run(async () => await this.resourceLockingAdapter.UnlockAsync(command.Id), cancellationToken)
            };

            await Task.WhenAll(unLockingTasks);
        }
    }
}