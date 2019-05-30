using System;
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
        private readonly IUserChildUrlFactory<ProfileImage> profileImageUrlFactory;

        public UserDtoTypeConverter(IUserChildUrlFactory<ProfileImage> profileImageUrlFactory)
        {
            this.profileImageUrlFactory = profileImageUrlFactory;
        }

        public UserDto Convert(User source, UserDto destination, ResolutionContext context)
        {
            var emails = source.Emails.Select(email => new EmailDto(email.Id, email.Address, email.IsPrimary))
                .ToArray();

            var profileImageMetaInfos = source.ProfileImages.Select(image =>
                    new ProfileImageMetaInfoDto(image.Id,
                        this.profileImageUrlFactory.GenerateUrl(image, RouteNames.ProfileImages.ProfileImageLoadRoute),
                        this.profileImageUrlFactory.GenerateUrl(image,
                            RouteNames.ProfileImages.ProfileImageDeleteRoute),
                        image.IsPrimary))
                .ToArray();

            return new UserDto(source.Id, source.UserName,
                source.FirstName,
                source.Name,
                source.Biography,
                source.RegisteredSince,
                source.AuthenticationProvider.ToString(),
                source.AuthenticationProvider != AuthenticationProvider.UsernamePassword,
                emails,
                profileImageMetaInfos
            );
        }
    }
}