using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.Users
{
    public class UserProfileInfoChangeCommandHandler : IVoidCommandHandler<UserProfileInfoChangeCommand>
    {
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;

        public UserProfileInfoChangeCommandHandler(IUsersService usersService,
            IResourceLockingAdapter resourceLockingAdapter)
        {
            this.usersService = usersService;
            this.resourceLockingAdapter = resourceLockingAdapter;
        }

        public async Task<Unit> Handle(UserProfileInfoChangeCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByIdAsync(command.Id);

            if (existingUser == null)
            {
                throw WellknownExceptions.UserNotFoundException();
            }

            if (!await this.resourceLockingAdapter.LockAsync(existingUser.Id, TimeSpan.FromSeconds(30)))
            {
                throw WellknownExceptions.UserResourceLockException(existingUser.Id);
            }

            var user = new User(existingUser.Id, existingUser.UserName, command.FirstName, command.Name,
                command.Biography, existingUser.RegisteredSince,
                existingUser.RoleIds, existingUser.Emails, existingUser.AuthenticationProvider, existingUser.Password,
                existingUser.Salt, existingUser.ProfileImages);

            await this.usersService.EditAsync(user);

            await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);

            return Unit.Value;
        }
    }
}