using System;

namespace Etdb.UserService.Services.Abstractions
{
    public interface IAuthenticationLogUrlFactory
    {
        string GenerateLoadAllUrl(Guid userId);
    }
}