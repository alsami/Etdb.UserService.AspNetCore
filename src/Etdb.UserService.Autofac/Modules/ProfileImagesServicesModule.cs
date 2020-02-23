using Autofac;
using Azure.Storage.Blobs;
using Etdb.UserService.Autofac.Extensions;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Etdb.UserService.Autofac.Modules
{
    public class ProfileImagesServicesModule : Module
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly IConfiguration? configuration;

        public ProfileImagesServicesModule(IHostEnvironment hostEnvironment, IConfiguration? configuration)
        {
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)        {
            if (!this.hostEnvironment.IsAnyAzure())
            {
                builder.RegisterType<FileProfileImageStorageService>()
                    .As<IProfileImageStorageService>()
                    .InstancePerLifetimeScope();
            
                builder.RegisterType<ProfileImageUrlFactory>()
                    .As<IProfileImageUrlFactory>()
                    .InstancePerLifetimeScope();

                return;
            }
            
            builder.Register(_ => new BlobServiceClient(this.configuration!.GetConnectionString("AzureProfileImagesContainer")))
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AzureProfileImageStorageService>()
                .As<IProfileImageStorageService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AzureProfileImageUrlFactory>()
                .As<IProfileImageUrlFactory>()
                .InstancePerLifetimeScope();
        }
    }
}