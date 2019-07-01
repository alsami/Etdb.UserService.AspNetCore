using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Cqrs.Misc;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Services.Abstractions;
using MediatR;

namespace Etdb.UserService.Cqrs.CommandHandler.ProfileImages
{
    public class ProfileImageSetPrimaryCommandHandler : IVoidCommandHandler<ProfileImageSetPrimaryCommand>
    {
        private readonly IUsersService usersService;
        private readonly IResourceLockingAdapter resourceLockingAdapter;

        public ProfileImageSetPrimaryCommandHandler(IUsersService usersService, IResourceLockingAdapter resourceLockingAdapter)
        {
            this.usersService = usersService;
            this.resourceLockingAdapter = resourceLockingAdapter;
        }

        public async Task<Unit> Handle(ProfileImageSetPrimaryCommand command, CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(command.UserId);

            if (user == null) throw WellknownExceptions.UserNotFoundException();

            if (!await this.resourceLockingAdapter.LockAsync(user.Id, TimeSpan.FromMinutes(1)))
                throw WellknownExceptions.UserResourceLockException(user.Id);

            var selectedImage = user.ProfileImages.FirstOrDefault(image => image.Id == command.Id);

            if (selectedImage == null) throw WellknownExceptions.ProfileImageNotFoundException();

            MutateCurrentPrimaryImage(user);
            
            MutateNewPrimaryImage(user, selectedImage);

            await this.usersService.EditAsync(user);

            await this.resourceLockingAdapter.UnlockAsync(user.Id);
            
            return Unit.Value;
        }

        private static void MutateNewPrimaryImage(User user, ProfileImage selectedImage)
        {
            var replacedImage = selectedImage.MutatePrimaryState(true);
            
            user.RemoveProfimeImage(image => image.Id == selectedImage.Id);
            
            user.AddProfileImage(replacedImage);
        }

        private static void MutateCurrentPrimaryImage(User user)
        {
            var primaryImage = user.ProfileImages.FirstOrDefault(image => image.IsPrimary);
            
            if (primaryImage == null) return;

            var replacedImage = primaryImage.MutatePrimaryState(false);

            user.RemoveProfimeImage(image => image.Id == primaryImage.Id);
            
            user.AddProfileImage(replacedImage);
        }
    }
}