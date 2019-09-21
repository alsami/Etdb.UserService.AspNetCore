using System.Linq;
using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Presentation.Users;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class UserFlatDtoTypeConverter : ITypeConverter<User, UserFlatDto>
    {
        private readonly IUserUrlFactory userUrlFactory;

        public UserFlatDtoTypeConverter(IUserUrlFactory userUrlFactory)
        {
            this.userUrlFactory = userUrlFactory;
        }

        public UserFlatDto Convert(User source, UserFlatDto destination, ResolutionContext context)
        {
            var selectedImage = source
                            .ProfileImages
                            .FirstOrDefault(image => image.IsPrimary)
                        ?? source.ProfileImages.FirstOrDefault();
            
            return new UserFlatDto(source.Id, source.UserName, selectedImage != null
                ? this.userUrlFactory.GenerateUrl(source, RouteNames.ProfileImages.LoadResizedRoute)
                : null, source.RegisteredSince);
        }
    }
}