using Rebus.Handlers;
using Sample.Shared;

namespace Sample.APIRequest.Handlers;

public class MyResponseHandler : IHandleMessages<MyResponse>
{
    public Task Handle(MyResponse message)
    {
        if (Dependency.responses.TryRemove(message.CorrelationId, out var tcs))
        {
            tcs.SetResult(message);
        }
        return Task.CompletedTask;
    }
}