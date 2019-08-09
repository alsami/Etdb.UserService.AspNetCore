using System;
using System.Security.Authentication;
using Autofac;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Etdb.UserService.Bootstrap.Extensions;
using Etdb.UserService.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Etdb.UserService.Bootstrap.AutofacModules
{
    public class DocumentDbContextModule : Module
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public DocumentDbContextModule(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(componentContext =>
                {
                    var options = componentContext.Resolve<IOptions<DocumentDbContextOptions>>();

                    if (!this.hostingEnvironment.IsAzureDevelopment()) return new UserServiceDbContext(options);

                    var settings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));

                    settings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = SslProtocols.Tls12
                    };

                    return new UserServiceDbContext(settings, options.Value.DatabaseName);
                })
                .As<DocumentDbContext>()
                .InstancePerLifetimeScope();
        }
    }
}