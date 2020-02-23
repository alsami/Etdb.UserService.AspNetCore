using System;
using System.Security.Authentication;
using Autofac;
using Etdb.UserService.Autofac.Extensions;
using Etdb.UserService.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Etdb.UserService.Autofac.Modules
{
    public class DocumentDbContextModule : Module
    {
        private readonly IHostEnvironment hostEnvironment;

        public DocumentDbContextModule(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserServiceDbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Register<Func<IMongoDatabase>>(componentContext =>
                {
                    return () =>
                    {
                        var options = componentContext.Resolve<IOptions<DocumentDbContextOptions>>();

                        var settings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));

                        if (this.hostEnvironment.IsAzureDevelopment())
                        {
                            settings.SslSettings = new SslSettings
                            {
                                EnabledSslProtocols = SslProtocols.Tls12
                            };
                        }

                        var client = new MongoClient(settings);

                        return client.GetDatabase(options.Value.DatabaseName);
                    };
                })
                .InstancePerLifetimeScope();
        }
    }
}