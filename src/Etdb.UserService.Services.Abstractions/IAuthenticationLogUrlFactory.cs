using System;
using Etdb.UserService.Domain.Entities;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IAuthenticationLogUrlFactory
    {
        string GenerateLoadAllUrl(Guid userId);
    }
}