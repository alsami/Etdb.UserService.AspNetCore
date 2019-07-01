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
        ProfileImageLoadCommandHandler : IResponseCommandHandler<ProfileImageLoadCommand, FileDownloadInfoDto>
    {
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private readonly IUsersService usersService;
        private readonly IFileService fileService;

        public ProfileImageLoadCommandHandler(IOptions<FilestoreConfiguration> fileStoreOptions,
            IUsersService usersService, IFileService fileService)
        {
            this.fileStoreOptions = fileStoreOptions;
            this.usersService = usersService;
            this.fileService = fileService;
        }

        public async Task<FileDownloadInfoDto> Handle(ProfileImageLoadCommand request,
            CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(request.UserId);

            if (user == null) throw new ResourceNotFoundException("The requested user could not be found!");

            var wantedImage = user.ProfileImages.FirstOrDefault(image => image.Id == request.Id);

            if (wantedImage == null)
                throw new ResourceNotFoundException("The requested profile-image was not found!");

            var binary = await this.fileService.ReadBinaryAsync(
                Path.Combine(this.fileStoreOptions.Value.ImagePath, wantedImage.RelativePath()));

            return new FileDownloadInfoDto(wantedImage.MediaType, wantedImage.Name, binary);
        }
    }
}