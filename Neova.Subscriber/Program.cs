// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using Neova.Subscriber;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Subscriber");

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://vqzzaxva:VtMK3VqNQT2_RhydLE5Sn-tIyrKYs0HQ@fish.rmq.cloudamqp.com/vqzzaxva")
};
var connection = connectionFactory.CreateConnection();

using var channel = connection.CreateModel();

//prefetch count

channel.BasicQos(0, 10, true);

var consumer = new EventingBasicConsumer(channel);

// event => delegate => method

consumer.Received += (sender, eventArgs) =>
{
    Thread.Sleep(1000);
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

    if (userCreatedEvent is not null)
    {
        Console.WriteLine(
            $"Message received:{userCreatedEvent.Id} - {userCreatedEvent.Name} - {userCreatedEvent.Email} ");

        channel.BasicAck(eventArgs.DeliveryTag, false);
    }
};


channel.BasicConsume("demo-queue", false, consumer);

Console.ReadLine();