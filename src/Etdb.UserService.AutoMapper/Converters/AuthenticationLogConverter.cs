using System;
using AutoMapper;
using Etdb.UserService.Cqrs.Abstractions.Events.Users;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Domain.ValueObjects;

namespace Etdb.UserService.AutoMapper.Converters
{
    public class AuthenticationLogConverter : ITypeConverter<UserAuthenticatedEvent, AuthenticationLog>
    {
        public AuthenticationLog Convert(UserAuthenticatedEvent source, AuthenticationLog destination,
            ResolutionContext context)
            => new AuthenticationLog(Guid.NewGuid(), source.UserId, source.LoggedAt,
                (AuthenticationLogType) Enum.Parse(typeof(AuthenticationLogType), source.AuthenticationLogType),
                source.IpAddress,
                source.AdditionalInfo);
    }
}