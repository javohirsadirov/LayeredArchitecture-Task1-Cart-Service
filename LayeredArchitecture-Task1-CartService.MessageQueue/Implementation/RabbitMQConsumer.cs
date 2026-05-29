using System.Text;
using LayeredArchitecture_Task1_CartService.MessageQueue.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LayeredArchitecture_Task1_CartService.MessageQueue.Implementation;

public class RabbitMQConsumer : IMessageConsumer
{
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly string _hostName;
    private readonly int _port;

    public RabbitMQConsumer(string hostName, int port)
    {
        _hostName = hostName;
        _port = port;
    }

    public async Task StartAsync(string exchange, string queue, string routingKey, Func<string, Task> onMessageReceived, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            Port = _port
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true, cancellationToken: cancellationToken);
        await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
        await _channel.QueueBindAsync(queue, exchange, routingKey, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            await onMessageReceived(body);
            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken);
        };

        await _channel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
    }

    public async Task StopAsync()
    {
        if (_channel is not null)
            await _channel.CloseAsync();
        if (_connection is not null)
            await _connection.CloseAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        if (_channel is not null)
            await _channel.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
