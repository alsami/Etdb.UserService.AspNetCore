using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands.ProfileImages;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.CommandHandler.ProfileImages
{
    public class
        ProfileImageResizedLoadCommandHandler : IResponseCommandHandler<ProfileImageResizedLoadCommand, FileDownloadInfoDto>
    {
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private readonly IUsersService usersService;
        private readonly IFileService fileService;
        private readonly IImageCompressionService imageCompressionService;

        public ProfileImageResizedLoadCommandHandler(IOptions<FilestoreConfiguration> fileStoreOptions,
            IUsersService usersService, IFileService fileService, IImageCompressionService imageCompressionService)
        {
            this.fileStoreOptions = fileStoreOptions;
            this.usersService = usersService;
            this.fileService = fileService;
            this.imageCompressionService = imageCompressionService;
        }

        public async Task<FileDownloadInfoDto> Handle(ProfileImageResizedLoadCommand command,
            CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(command.UserId);

            if (user == null) throw new ResourceNotFoundException("The requested user could not be found!");

            var wantedImage = user.ProfileImages.FirstOrDefault(image => image.Id == command.Id);

            if (wantedImage == null)
                throw new ResourceNotFoundException("The requested profile-image was not found!");

            var binary = await this.fileService.ReadBinaryAsync(
                Path.Combine(this.fileStoreOptions.Value.ImagePath, wantedImage.RelativePath()));

            var thumbnailBinary = this.imageCompressionService.Resize(binary,
                wantedImage.MediaType == "image/*" ? "image/jpeg" : wantedImage.MediaType, command.DimensionX, command.DimensionY);

            return new FileDownloadInfoDto(wantedImage.MediaType, wantedImage.Name, thumbnailBinary);
        }
    }
}