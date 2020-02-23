using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.ProfileImages
{
    public class ProfileImageRemoveCommandHandler : IRequestHandler<ProfileImageRemoveCommand>
    {
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;
        private readonly IProfileImageStorageService profileImageStorageService;

        public ProfileImageRemoveCommandHandler(IUsersService usersService,
            IResourceLockingAdapter resourceLockingAdapter, IProfileImageStorageService profileImageStorageService)
        {
            this.usersService = usersService;
            this.resourceLockingAdapter = resourceLockingAdapter;
            this.profileImageStorageService = profileImageStorageService;
        }

        public async Task<Unit> Handle(ProfileImageRemoveCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await this.usersService.FindByIdAsync(command.UserId);

            if (existingUser == null) throw WellknownExceptions.UserNotFoundException();

            if (!await this.resourceLockingAdapter.LockAsync(existingUser.Id, TimeSpan.FromSeconds(30))) throw WellknownExceptions.UserResourceLockException(existingUser.Id);

            var imageToDelete = existingUser.ProfileImages.FirstOrDefault(image => image.Id == command.Id);

            if (imageToDelete == null) throw WellknownExceptions.ProfileImageNotFoundException();

            await this.profileImageStorageService.RemoveAsync(imageToDelete);

            existingUser.RemoveProfimeImage(image => image.Id == command.Id);

            await this.usersService.EditAsync(existingUser);

            await this.resourceLockingAdapter.UnlockAsync(existingUser.Id);

            return Unit.Value;
        }
    }
}