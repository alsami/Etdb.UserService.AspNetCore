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
        private readonly IUserChildUrlFactory<Email> emailUrlFactory;

        public UserDtoTypeConverter(IUserChildUrlFactory<ProfileImage> profileImageUrlFactory, IUserChildUrlFactory<Email> emailUrlFactory)
        {
            this.profileImageUrlFactory = profileImageUrlFactory;
            this.emailUrlFactory = emailUrlFactory;
        }

        public UserDto Convert(User source, UserDto destination, ResolutionContext context)
        {
            
            var emaiMetaInfos = source.Emails.Select(email => new EmailMetaInfoDto(email.Id,
                    this.emailUrlFactory.GenerateUrl(email, RouteNames.ProfileImages.LoadRoute),
                    this.emailUrlFactory.GenerateUrl(email,
                        RouteNames.ProfileImages.DeleteRoute),
                    email.IsPrimary, email.IsExternal))
                .ToArray();
            
            var emailMetaInfoContainer = new EmailMentaInfoContainer(this.emailUrlFactory.GenerateUrl(source.Emails.First(), RouteNames.Emails.LoadAllRoute), emaiMetaInfos);

            var profileImageMetaInfos = source.ProfileImages.Select(image =>
                    new ProfileImageMetaInfoDto(image.Id,
                        this.profileImageUrlFactory.GenerateUrl(image, RouteNames.ProfileImages.LoadRoute),
                        this.profileImageUrlFactory.GenerateUrl(image,
                            RouteNames.ProfileImages.DeleteRoute),
                        image.IsPrimary))
                .ToArray();

            return new UserDto(source.Id, source.UserName,
                source.FirstName,
                source.Name,
                source.Biography,
                source.RegisteredSince,
                source.AuthenticationProvider.ToString(),
                source.AuthenticationProvider != AuthenticationProvider.UsernamePassword,
                emailMetaInfoContainer,
                profileImageMetaInfos
            );
        }
    }
}