using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class ProfileImageConverter : ITypeConverter<ProfileImage, ProfileImageMetaInfoDto>
    {
        private readonly IProfileImageUrlFactory profileImageUrlFactory;

        public ProfileImageConverter(IProfileImageUrlFactory profileImageUrlFactory) =>
            this.profileImageUrlFactory = profileImageUrlFactory;

        public ProfileImageMetaInfoDto Convert(ProfileImage source, ProfileImageMetaInfoDto destination,
            ResolutionContext context)
            => new ProfileImageMetaInfoDto(source.Id,
                this.profileImageUrlFactory.GenerateUrl(source),
                this.profileImageUrlFactory.GetResizeUrl(source),
                this.profileImageUrlFactory.GetDeleteUrl(source),
                source.IsPrimary, source.CreatedAt);
    }
}