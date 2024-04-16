// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using Neova.Publisher;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Publisher");

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://vqzzaxva:VtMK3VqNQT2_RhydLE5Sn-tIyrKYs0HQ@fish.rmq.cloudamqp.com/vqzzaxva")
};
var connection = connectionFactory.CreateConnection();

using var channel = connection.CreateModel();


channel.BasicReturn += (object? sender, BasicReturnEventArgs e) =>
{
    Console.WriteLine($"Mesaj gitmedi : {e.BasicProperties.MessageId}");
};


channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct, true, false, null);

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
        channel.BasicPublish("demo-direct-exchange", "route-key-a", true, null, userCreatedEventAsBinary);
        Console.WriteLine("Message sent: ");
        channel.WaitForConfirms(TimeSpan.FromSeconds(10));
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
});