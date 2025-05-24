namespace Sample.Shared;

public static class MyConfig
{
    public static string RabbitMqUrl => "amqp://guest:guest@localhost";
    public static string ApiRequestQueueA => "api-a-queue";
    public static string ApiResponseQueueB = "api-b-queue";
}
