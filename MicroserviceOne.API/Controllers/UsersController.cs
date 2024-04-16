using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

namespace MicroserviceOne.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IPublishEndpoint publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            // save to database
            var userCreatedEvent = new UserCreatedEvent() { Id = 10, Email = "ahmet@outlook.com", Name = "Ahmet" };


            await publishEndpoint.Publish(userCreatedEvent);

            return Ok();
        }
    }
}