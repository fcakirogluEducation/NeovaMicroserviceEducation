using MassTransit;
using Shared.Events;

namespace MicroserviceTwo.API.Consumers
{
    public class UserCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            Console.WriteLine("UserCreatedEventConsumer Çalıştı");


            throw new Exception("Veri tabanına bağlanamadı. ");
            var id = context.Headers.Get("id", string.Empty);
            var version = context.Headers.Get("version", string.Empty);


            Console.WriteLine($"header :{id}- {version}");
            Console.WriteLine($"{context.Message.Id} - {context.Message.Name} - {context.Message.Email}");

            var hasStock = true;
            if (hasStock)
            {
                // new event
            }
            else
            {
            }


            // db.

            return Task.CompletedTask;
        }
    }
}