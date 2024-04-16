using MassTransit;
using Shared.Events;

namespace MicroserviceTwo.API.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            Console.WriteLine($"{context.Message.Id} - {context.Message.Name} - {context.Message.Email}");
            return Task.CompletedTask;
        }
    }
}