using System;
using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Events.Authentication;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class SignInLogConverter : ITypeConverter<UserSignedInEvent, SignInLog>
    {
        public SignInLog Convert(UserSignedInEvent source, SignInLog destination, ResolutionContext context)
            => new SignInLog(Guid.NewGuid(), source.UserId, source.LoggedAt,
                (SignInType) Enum.Parse(typeof(SignInType), source.SignInEventType), source.IpAddress?.ToString(),
                source.AdditionalInfo);
    }
}