using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.CommandHandler.ProfileImages
{
    public class ProfileImageRemoveCommandHandler : IVoidCommandHandler<ProfileImageRemoveCommand>
    {
        private readonly IUsersService usersService;
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IFileService fileService;

        public ProfileImageRemoveCommandHandler(IUsersService usersService,
            IResourceLockingAdapter resourceLockingAdapter, IFileService fileService,
            IOptions<FilestoreConfiguration> fileStoreOptions)
        {
            this.usersService = usersService;
            this.resourceLockingAdapter = resourceLockingAdapter;
            this.fileService = fileService;
            this.fileStoreOptions = fileStoreOptions;
        }

        public async Task<Unit> Handle(ProfileImageRemoveCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByIdAsync(command.UserId);

            if (existingUser == null)
            {
                throw WellknownExceptions.UserNotFoundException();
            }

            if (!await this.resourceLockingAdapter.LockAsync(existingUser.Id, TimeSpan.FromSeconds(30)))
            {
                throw WellknownExceptions.UserResourceLockException(existingUser.Id);
            }

            var imageToDelete = existingUser.ProfileImages.FirstOrDefault(image => image.Id == command.Id);

            if (imageToDelete == null) throw WellknownExceptions.ProfileImageNotFoundException();

            this.fileService.DeleteBinary(Path.Combine(this.fileStoreOptions.Value.ImagePath, imageToDelete.SubPath()),
                imageToDelete.Name);

            existingUser.RemoveProfimeImage(image => image.Id == command.Id);

            await this.usersService.EditAsync(existingUser);

            await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);

            return Unit.Value;
        }
    }
}