using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

namespace MicroserviceOne.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            // begin transaction
            // order to order_table
            // new outbox to outbox_table
            // end


            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            var sendEndpoint =
                await sendEndpointProvider.GetSendEndpoint(new Uri("queue:microservice-two.user-created-event-queue"));


            for (int i = 0; i < 10; i++)
            {
                var userCreatedEvent = new UserCreatedEvent() { Id = i, Email = "ahmet@outlook.com", Name = "Ahmet" };
                //publishEndpoint.Publish(userCreatedEvent, x => { x.SetAwaitAck(true); },
                //    cancellationTokenSource.Token);
                await sendEndpoint.Send(userCreatedEvent, x => { x.SetAwaitAck(true); },
                    cancellationToken: cancellationTokenSource.Token);
            }

            //var userCreatedEvent = new UserCreatedEvent() { Id = 1, Email = "ahmet@outlook.com", Name = "Ahmet" };
            //await publishEndpoint.Publish(userCreatedEvent, x =>
            //{
            //    x.Durable = true;
            //    x.Mandatory = true;
            //    x.Headers.Set("version", "v1");
            //    x.Headers.Set("id", Guid.NewGuid());
            //    x.Headers.Set("creted", DateTime.Now);
            //    x.SetAwaitAck(true);
            //}, cancellationTokenSource.Token);


            //var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:microservice-two.send-email"));

            //await sendEndpoint.Send(new SendEmailMessage("mehmet@outlook.com"), x => { x.SetAwaitAck(true); });

            return Ok();
        }
    }
}