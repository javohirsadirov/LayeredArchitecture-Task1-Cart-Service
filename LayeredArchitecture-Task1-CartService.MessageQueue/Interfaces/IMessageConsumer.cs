namespace LayeredArchitecture_Task1_CartService.MessageQueue.Interfaces;

public interface IMessageConsumer : IAsyncDisposable
{
    Task StartAsync(string exchange, string queue, string routingKey, Func<string, Task> onMessageReceived, CancellationToken cancellationToken);
    Task StopAsync();
}