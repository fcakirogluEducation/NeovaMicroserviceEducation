using MassTransit;
using MicroserviceTwo.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedEventConsumer>();
    x.AddConsumer<SendEmailMessageConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        // 1. seviye retry
        //cfg.UseMessageRetry(r=>r.Immediate(3));
        cfg.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5)));

        //cfg.PrefetchCount = 20;
        //2. seviye retry

        //cfg.UseDelayedRedelivery(r=>r.Intervals(TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(60)));

        //// outbox
        //cfg.UseInMemoryOutbox(context);


        cfg.Host("localhost", "/");


        cfg.ReceiveEndpoint("microservice-two.user-created-event-queue",
            e =>
            {
                e.ConcurrentMessageLimit = 1;
                //e.UseMessageRetry(r=>r.Immediate(5));
                e.ConfigureConsumer<UserCreatedEventConsumer>(context);
            }
        );

        cfg.ReceiveEndpoint("microservice-two.send-email",
            e => { e.ConfigureConsumer<SendEmailMessageConsumer>(context); });
        //
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();