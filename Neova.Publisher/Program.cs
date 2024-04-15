// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using Neova.Publisher;
using RabbitMQ.Client;

Console.WriteLine("Publisher");

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://vqzzaxva:VtMK3VqNQT2_RhydLE5Sn-tIyrKYs0HQ@fish.rmq.cloudamqp.com/vqzzaxva")
};
var connection = connectionFactory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.ConfirmSelect();


Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    var userCreatedEvent = new UserCreatedEvent
    {
        Id = x,
        Name = "Ali",
        Email = "ali@outlook.com"
    };
    var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);


    var userCreatedEventAsBinary = Encoding.UTF8.GetBytes(userCreatedEventAsJson);

    try
    {
        channel.BasicPublish("", "demo-queue", null, userCreatedEventAsBinary);

        channel.WaitForConfirms(TimeSpan.FromSeconds(10));
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }


    Console.WriteLine("Message sent: ");
});