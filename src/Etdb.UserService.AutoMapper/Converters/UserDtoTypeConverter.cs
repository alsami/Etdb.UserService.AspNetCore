using System.Linq;
using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class UserDtoTypeConverter : ITypeConverter<User, UserDto>
    {
        private readonly IUserUrlFactory profileImageUrlFactory;

        private readonly IUserUrlFactory authenticationLogUrlFactory;

        public UserDtoTypeConverter(IUserUrlFactory profileImageUrlFactory,
            IUserUrlFactory authenticationLogUrlFactory)
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
                        this.profileImageUrlFactory.GenerateUrlWithChildIdParameter(image,
                            RouteNames.ProfileImages.LoadRoute),
                        this.profileImageUrlFactory.GenerateUrlWithChildIdParameter(image,
                            RouteNames.ProfileImages.LoadResizedRoute),
                        this.profileImageUrlFactory.GenerateUrlWithChildIdParameter(image,
                            RouteNames.ProfileImages.DeleteRoute),
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
                this.authenticationLogUrlFactory.GenerateUrl(source, RouteNames.AuthenticationLogs.LoadAllRoute)
            );
        }
    }
}