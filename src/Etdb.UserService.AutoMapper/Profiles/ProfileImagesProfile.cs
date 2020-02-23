using AutoMapper;
using Etdb.UserService.AutoMapper.Converters;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Presentation.Users;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class ProfileImagesProfile : Profile
    {
        public ProfileImagesProfile()
        {
            this.CreateMap<ProfileImage, ProfileImageMetaInfoDto>()
                .ConvertUsing<ProfileImageConverter>();
        }
    }
}