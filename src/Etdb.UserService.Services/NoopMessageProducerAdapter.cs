using System.Threading.Tasks;
using Etdb.UserService.Services.Abstractions;

namespace Etdb.UserService.Services
{
    // ReSharper disable once UnusedType.Global
    public class NoopMessageProducerAdapter : IMessageProducerAdapter
    {
        public Task ProduceAsync<TMessage>(TMessage message, MessageType messageType) where TMessage : class => Task.CompletedTask;
    }
}