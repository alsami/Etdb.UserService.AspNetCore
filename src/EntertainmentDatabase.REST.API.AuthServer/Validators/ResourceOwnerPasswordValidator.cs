using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.API.AuthServer.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "test123" && context.Password == "password")
            {
                context.Result = new GrantValidationResult(subject: "81822", authenticationMethod: "custom");
                return Task.FromResult(context.Result);
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            return Task.FromResult(context.Result);
        }
    }
}
