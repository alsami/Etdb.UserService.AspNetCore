using System.Linq;
using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class UserFlatDtoTypeConverter : ITypeConverter<User, UserFlatDto>
    {
        private readonly IProfileImageUrlFactory userUrlFactory;

        public UserFlatDtoTypeConverter(IProfileImageUrlFactory userUrlFactory)
        {
            this.userUrlFactory = userUrlFactory;
        }

        public UserFlatDto Convert(User source, UserFlatDto destination, ResolutionContext context)
        {
            var selectedImage = source.ProfileImages.FirstOrDefault(image => image.IsPrimary) ??
                                source.ProfileImages.FirstOrDefault();

            return new UserFlatDto(source.Id, source.UserName, selectedImage != null
                ? this.userUrlFactory.GetResizeUrl(selectedImage, source.Id)
                : null, source.RegisteredSince);
        }
    }
}