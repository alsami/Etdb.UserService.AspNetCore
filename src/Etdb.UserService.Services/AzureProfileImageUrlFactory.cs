using System;
using Azure.Storage.Blobs;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Etdb.UserService.Services
{
    public class AzureProfileImageUrlFactory : IProfileImageUrlFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;
        private readonly BlobServiceClient blobServiceClient;
        private const string ContainerName = "profileimages";

        public AzureProfileImageUrlFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, BlobServiceClient blobServiceClient)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
            this.blobServiceClient = blobServiceClient;
        }
        
        public string GenerateUrl(ProfileImage profileImage)
        {
            var containerClient = this.blobServiceClient.GetBlobContainerClient(ContainerName);

            var blobClient =
                containerClient.GetBlobClient($"{profileImage.UserId}/{profileImage.Name}");

            var uri = new Uri(containerClient.Uri, blobClient.Uri.AbsolutePath);

            return uri.AbsoluteUri;
        }

        public string GetResizeUrl(ProfileImage profileImage)
            => this.GenerateUrl(profileImage);
        
        public string GetDeleteUrl(ProfileImage profileImage)
        {
            var url = this.linkGenerator.GetUriByName(this.httpContextAccessor.HttpContext, RouteNames.ProfileImages.DeleteRoute, new
            {
                userId = profileImage.UserId,
                id = profileImage.Id,
            });

            return url;
        }
    }
}