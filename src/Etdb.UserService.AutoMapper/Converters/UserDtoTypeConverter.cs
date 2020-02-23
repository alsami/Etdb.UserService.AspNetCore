using System.Linq;
using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class UserDtoTypeConverter : ITypeConverter<User, UserDto>
    {
        private readonly IProfileImageUrlFactory profileImageUrlFactory;

        private readonly IAuthenticationLogUrlFactory authenticationLogUrlFactory;

        public UserDtoTypeConverter(IProfileImageUrlFactory profileImageUrlFactory,
            IAuthenticationLogUrlFactory authenticationLogUrlFactory)
        {
            this.profileImageUrlFactory = profileImageUrlFactory;
            this.authenticationLogUrlFactory = authenticationLogUrlFactory;
        }

        public UserDto Convert(User source, UserDto destination, ResolutionContext context)
        {
            var profileImageMetaInfos = source.ProfileImages
                .OrderByDescending(image => image.CreatedAt)
                .Select(image =>
                    new ProfileImageMetaInfoDto(image.Id,
                        this.profileImageUrlFactory.GenerateUrl(image, source.Id),
                        this.profileImageUrlFactory.GetResizeUrl(image, source.Id),
                        this.profileImageUrlFactory.GetDeleteUrl(image, source.Id),
                        image.IsPrimary, image.CreatedAt))
                .ToArray();

            return new UserDto(source.Id, source.UserName,
                source.FirstName,
                source.Name,
                source.Biography,
                source.RegisteredSince,
                source.AuthenticationProvider.ToString(),
                source.AuthenticationProvider != AuthenticationProvider.UsernamePassword,
                null,
                profileImageMetaInfos,
                this.authenticationLogUrlFactory.GenerateLoadAllUrl(source.Id)
            );
        }
    }
}