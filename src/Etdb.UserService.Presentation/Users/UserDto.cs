using System;
using System.Collections.Generic;
using Etdb.UserService.Presentation.Base;

// ReSharper disable UnusedMember.Global

namespace Etdb.UserService.Presentation.Users
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class UserDto : GuidDto
    {
        public string UserName { get; }

        public string FirstName { get; }

        public string Name { get; }

        public string Biography { get; }

        public DateTime RegisteredSince { get; }

        public string AuthenticationProvider { get; }

        public bool IsExternalUser { get; }

        public EmailMentaInfoContainer EmailMentaInfoContainer { get; }

        public ICollection<ProfileImageMetaInfoDto> ProfileImageMetaInfos { get; }

        public string AuthenticationLogsUrl { get; }

        public UserDto(Guid id, string userName, string firstName, string name, string biography,
            DateTime registeredSince, string authenticationProvider, bool isExternalUser,
            EmailMentaInfoContainer emailMentaInfoContainer, ICollection<ProfileImageMetaInfoDto> profileImageMetaInfos,
            string authenticationLogsUrl) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
            this.RegisteredSince = registeredSince;
            this.AuthenticationProvider = authenticationProvider;
            this.IsExternalUser = isExternalUser;
            this.EmailMentaInfoContainer = emailMentaInfoContainer;
            this.ProfileImageMetaInfos = profileImageMetaInfos;
            this.AuthenticationLogsUrl = authenticationLogsUrl;
        }
    }
}