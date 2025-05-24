using System.Collections.Concurrent;

namespace Sample.Shared;

public static class Dependency
{
    public static readonly ConcurrentDictionary<string, TaskCompletionSource<MyResponse>> responses = new();
}
