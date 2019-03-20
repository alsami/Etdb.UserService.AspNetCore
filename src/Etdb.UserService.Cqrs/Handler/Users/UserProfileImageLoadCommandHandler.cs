using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.Exceptions;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Cqrs.Abstractions.Commands.Users;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Presentation;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.Cqrs.Handler.Users
{
    public class
        UserProfileImageLoadCommandHandler : IResponseCommandHandler<UserProfileImageLoadCommand, FileDownloadInfoDto>
    {
        private readonly IOptions<FilestoreConfiguration> fileStoreOptions;
        private readonly IUsersService usersService;
        private readonly IFileService fileService;

        public UserProfileImageLoadCommandHandler(IOptions<FilestoreConfiguration> fileStoreOptions,
            IUsersService usersService, IFileService fileService)
        {
            this.fileStoreOptions = fileStoreOptions;
            this.usersService = usersService;
            this.fileService = fileService;
        }

        public async Task<FileDownloadInfoDto> Handle(UserProfileImageLoadCommand request,
            CancellationToken cancellationToken)
        {
            var user = await this.usersService.FindByIdAsync(request.Id);

            if (user == null)
            {
                throw new ResourceNotFoundException("The requested user could not be found!");
            }

            if (user.ProfileImage == null)
            {
                throw new ResourceNotFoundException("The requested user does not have a profile image!");
            }

            var binary = await this.fileService.ReadBinaryAsync(
                Path.Combine(this.fileStoreOptions.Value.ImagePath, user.Id.ToString()), user.ProfileImage.Name);

            return new FileDownloadInfoDto(user.ProfileImage.MediaType, user.ProfileImage.Name, binary);
        }
    }
}