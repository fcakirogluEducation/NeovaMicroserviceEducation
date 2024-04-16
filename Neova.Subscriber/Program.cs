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

channel.BasicQos(0, 50, true);


// create queue
channel.QueueDeclare("queue-with-demo-direct-exchange", true, false, false, null);
//channel.QueueDeclare("queue2-with-demo-fanout-exchange", true, false, false, null);
//channel.QueueDeclare("queue3-with-demo-fanout-exchange", true, false, false, null);
// bind exchange
channel.QueueBind("queue-with-demo-direct-exchange", "demo-direct-exchange", "route-key-b");
//channel.QueueBind("queue2-with-demo-fanout-exchange", "demo-fanout-exchange", string.Empty);
//channel.QueueBind("queue3-with-demo-fanout-exchange", "demo-fanout-exchange", string.Empty);


var consumer = new EventingBasicConsumer(channel);


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

// event => delegate => method
channel.BasicConsume("queue-with-demo-direct-exchange", false, consumer);


Console.ReadLine();