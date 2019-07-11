using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class ProfileImageConverter : ITypeConverter<ProfileImage, ProfileImageMetaInfoDto>
    {
        private readonly IUserUrlFactory profileImageUrlFactory;

        public ProfileImageConverter(IUserUrlFactory profileImageUrlFactory) =>
            this.profileImageUrlFactory = profileImageUrlFactory;

        public ProfileImageMetaInfoDto Convert(ProfileImage source, ProfileImageMetaInfoDto destination,
            ResolutionContext context)
            => new ProfileImageMetaInfoDto(source.Id,
                this.profileImageUrlFactory.GenerateUrlWithChildIdParameter(source, RouteNames.ProfileImages.LoadRoute),
                this.profileImageUrlFactory.GenerateUrlWithChildIdParameter(source, RouteNames.ProfileImages.LoadResizedRoute),
                this.profileImageUrlFactory.GenerateUrlWithChildIdParameter(source,
                    RouteNames.ProfileImages.DeleteRoute),
                source.IsPrimary, source.CreatedAt);
    }
}