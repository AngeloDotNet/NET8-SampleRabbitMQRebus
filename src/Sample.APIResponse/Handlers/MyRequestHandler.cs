using Rebus.Bus;
using Rebus.Handlers;
using Sample.Shared;

namespace Sample.APIResponse.Handlers;

public class MyRequestHandler(IBus bus) : IHandleMessages<MyRequest>
{
    public async Task Handle(MyRequest message)
    {
        // Rispondi sulla coda ReplyTo
        await bus.Advanced.Routing.Send(message.ReplyTo, new MyResponse
        {
            Result = $"Risposta da API B! Ricevuto: {message.Text} in data: {DateTime.Now:dd/MM/yyyy} alle ore: {DateTime.Now:HH:mm}",
            CorrelationId = message.CorrelationId
        });
    }
}