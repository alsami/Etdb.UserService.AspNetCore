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
            var user = await this.usersService.FindByIdAsync(command.UserId);

            if (user == null) throw WellknownExceptions.UserNotFoundException();

            if (!await this.resourceLockingAdapter.LockAsync(user.Id, TimeSpan.FromSeconds(30))) throw WellknownExceptions.UserResourceLockException(user.Id);

            var imageToDelete = user.ProfileImages.FirstOrDefault(image => image.Id == command.Id);

            if (imageToDelete == null) throw WellknownExceptions.ProfileImageNotFoundException();

            await this.profileImageStorageService.RemoveAsync(imageToDelete, user.Id);

            user.RemoveProfimeImage(image => image.Id == command.Id);

            await this.usersService.EditAsync(user);

            await this.resourceLockingAdapter.UnlockAsync(user.Id);

            return Unit.Value;
        }
    }
}