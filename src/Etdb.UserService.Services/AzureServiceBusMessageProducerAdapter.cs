using System;
using System.Text;
using System.Threading.Tasks;
using Etdb.UserService.Services.Abstractions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;

namespace Etdb.UserService.Services
{
    public class AzureServiceBusMessageProducerAdapter : IMessageProducerAdapter, IAsyncDisposable
    {
        private readonly Func<MessageType, IMessageSender> messageSenderComposer;
        private IMessageSender messageSender;

        public AzureServiceBusMessageProducerAdapter(Func<MessageType, IMessageSender> messageSenderComposer)
        {
            this.messageSenderComposer = messageSenderComposer;
        }

        public async ValueTask DisposeAsync()
        {
            if (this.messageSender == null) return;

            await this.messageSender?.CloseAsync();
        }

        public async Task ProduceAsync<TMessage>(TMessage message, MessageType messageType) where TMessage : class
        {
            var serialized = JsonConvert.SerializeObject(message);

            var bytes = Encoding.UTF8.GetBytes(serialized);

            var sendableMessage = new Message(bytes);

            this.messageSender = this.messageSenderComposer(messageType);

            await this.messageSender.SendAsync(sendableMessage);
        }
    }
}