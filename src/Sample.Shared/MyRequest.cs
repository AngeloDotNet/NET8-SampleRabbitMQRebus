namespace Sample.Shared;

public class MyRequest
{
    public string Text { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
    public string ReplyTo { get; set; } = string.Empty;
}