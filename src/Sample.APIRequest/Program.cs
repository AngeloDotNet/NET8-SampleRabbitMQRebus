
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Sample.APIRequest.Handlers;
using Sample.Shared;

namespace Sample.APIRequest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services
            .AddRebus(configure => configure
            .Transport(transport => transport.UseRabbitMq(MyConfig.RabbitMqUrl, MyConfig.ApiRequestQueueA))
            .Routing(routing => routing.TypeBased().Map<MyRequest>(MyConfig.ApiResponseQueueB))
        );

        builder.Services.AutoRegisterHandlersFromAssemblyOf<MyResponseHandler>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapGet("/ask", async (string text, IBus bus) =>
        {
            var correlationId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<MyResponse>();
            Dependency.responses[correlationId] = tcs;

            await bus.Send(new MyRequest
            {
                Text = text,
                CorrelationId = correlationId,
                ReplyTo = "api-a-queue"
            });

            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(10000));

            if (completedTask == tcs.Task)
            {
                return Results.Ok(tcs.Task.Result.Result);
            }

            Dependency.responses.TryRemove(correlationId, out _);
            return Results.Problem("Timeout in attesa della risposta");
        });

        app.Run();
    }
}