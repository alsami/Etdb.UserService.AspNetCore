using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Extensions;
using Etdb.UserService.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler
{
    public class UserProfileImageRemoveCommandHandler : IVoidCommandHandler<UserProfileImageRemoveCommand>
    {
        private readonly IUsersService usersService;
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IFileService fileService;

        public UserProfileImageRemoveCommandHandler(IUsersService usersService,
            IResourceLockingAdapter resourceLockingAdapter, IFileService fileService,
            IOptions<FilestoreConfiguration> fileStoreOptions)
        {
            this.usersService = usersService;
            this.resourceLockingAdapter = resourceLockingAdapter;
            this.fileService = fileService;
            this.fileStoreOptions = fileStoreOptions;
        }

        public async Task<Unit> Handle(UserProfileImageRemoveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByIdAsync(request.Id);

            if (existingUser == null)
            {
                throw WellknownExceptions.UserNotFoundException();
            }

            if (!await this.resourceLockingAdapter.LockAsync(existingUser.Id, TimeSpan.FromSeconds(30)))
            {
                throw WellknownExceptions.UserResourceLockException(existingUser.Id);
            }

            this.fileService.DeleteBinary(Path.Combine(this.fileStoreOptions.Value.ImagePath,
                existingUser.Id.ToString(),
                existingUser.ProfileImage.Name));

            var updatedUser = new User(existingUser.Id, existingUser.UserName, existingUser.FirstName,
                existingUser.Name, existingUser.Biography,
                existingUser.RegisteredSince, existingUser.RoleIds, existingUser.Emails,
                existingUser.AuthenticationProvider, existingUser.Password, existingUser.Salt);

            await this.usersService.EditAsync(updatedUser);

            await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);


            return Unit.Value;
        }
    }
}