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
        public string UserName { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? Name { get; set; }

        public string? Biography { get; set; }

        public DateTime RegisteredSince { get; set; }

        public string AuthenticationProvider { get; set; } = null!;

        public bool IsExternalUser { get; set; }

        public EmailMentaInfoContainerDto? EmailMentaInfoContainerDto { get; set; }

        public ICollection<ProfileImageMetaInfoDto> ProfileImageMetaInfos { get; set; } = null!;

        public string AuthenticationLogsUrl { get; set; } = null!;

        public UserDto(Guid id, string userName, string? firstName, string? name, string? biography,
            DateTime registeredSince, string authenticationProvider, bool isExternalUser,
            EmailMentaInfoContainerDto? emailMentaInfoContainerDto,
            ICollection<ProfileImageMetaInfoDto> profileImageMetaInfos,
            string authenticationLogsUrl) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.Name = name;
            this.Biography = biography;
            this.RegisteredSince = registeredSince;
            this.AuthenticationProvider = authenticationProvider;
            this.IsExternalUser = isExternalUser;
            this.EmailMentaInfoContainerDto = emailMentaInfoContainerDto;
            this.ProfileImageMetaInfos = profileImageMetaInfos;
            this.AuthenticationLogsUrl = authenticationLogsUrl;
        }

        public UserDto()
        {
        }
    }
}