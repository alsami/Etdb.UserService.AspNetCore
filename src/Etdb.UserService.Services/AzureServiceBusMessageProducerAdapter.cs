using System;
using System.Text;
using System.Threading.Tasks;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;

namespace Etdb.UserService.Services
{
    public class AzureServiceBusMessageProducerAdapter : IMessageProducerAdapter
    {
        private readonly Func<MessageType, IMessageSender> messageSenderComposer;

        public AzureServiceBusMessageProducerAdapter(Func<MessageType, IMessageSender> messageSenderComposer)
        {
            this.messageSenderComposer = messageSenderComposer;
        }

        public async Task ProduceAsync<TMessage>(TMessage message, MessageType messageType) where TMessage : class
        {
            var serialized = JsonConvert.SerializeObject(message);

            var bytes = Encoding.UTF8.GetBytes(serialized);

            var sendableMessage = new Message(bytes);

            var sender = this.messageSenderComposer(messageType);

            await sender.SendAsync(sendableMessage);

            await sender.CloseAsync();
        }
    }
}