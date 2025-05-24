using Rebus.Config;
using Rebus.Routing.TypeBased;
using Sample.APIResponse.Handlers;
using Sample.Shared;

namespace Sample.APIResponse;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services
            .AddRebus(configure => configure
            .Transport(transport => transport.UseRabbitMq(MyConfig.RabbitMqUrl, MyConfig.ApiResponseQueueB))
            .Routing(routing => routing.TypeBased().Map<MyResponse>(MyConfig.ApiRequestQueueA)));

        builder.Services.AutoRegisterHandlersFromAssemblyOf<MyRequestHandler>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.Run();
    }
}
