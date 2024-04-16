using MassTransit;
using Shared.Events;

namespace MicroserviceTwo.API.Consumers
{
    public class SendEmailMessageConsumer(ILogger<SendEmailMessageConsumer> logger) : IConsumer<SendEmailMessage>
    {
        public Task Consume(ConsumeContext<SendEmailMessage> context)
        {
            logger.LogInformation($"Gelen Mesaj :{context.Message.Email}");

            return Task.CompletedTask;
        }
    }
}