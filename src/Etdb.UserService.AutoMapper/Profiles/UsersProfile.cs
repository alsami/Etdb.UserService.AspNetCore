using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Domain;
using Etdb.UserService.Presentation;

namespace Etdb.UserService.AutoMapper.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            this.CreateMap<User, UserDto>();

            this.CreateMap<Email, EmailDto>();

            this.CreateMap<UserRegisterCommand, User>();

            this.CreateMap<EmailAddCommand, Email>();
        }
    }
}