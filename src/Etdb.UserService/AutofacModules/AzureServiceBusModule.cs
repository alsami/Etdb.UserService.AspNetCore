using System;
using Autofac;
using Etdb.UserService.Misc.Configuration;
using Etdb.UserService.Services;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Options;

namespace Etdb.UserService.AutofacModules
{
    public class AzureServiceBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AzureServiceBusMessageProducerAdapter>()
                .As<IMessageProducerAdapter>()
                .InstancePerLifetimeScope();

            builder
                .Register<Func<MessageType, IMessageSender>>(componentContext =>
                {
                    var innerContext = componentContext.Resolve<IComponentContext>();

                    return messageType =>
                    {
                        var options = innerContext.Resolve<IOptions<AzureServiceBusConfiguration>>();

                        return messageType switch
                        {
                            MessageType.UserRegistered => new MessageSender(options.Value.ConnectionString,
                                options.Value.UserRegisteredTopic),
                            MessageType.UserAuthenticated => new MessageSender(options.Value.ConnectionString,
                                options.Value.UserAuthenticatedTopic),
                            _ => throw new ArgumentOutOfRangeException(nameof(messageType))
                        };
                    };
                })
                .InstancePerDependency();
        }
    }
}