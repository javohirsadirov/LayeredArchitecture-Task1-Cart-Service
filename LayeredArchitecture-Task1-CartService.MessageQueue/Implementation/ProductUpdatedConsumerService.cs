using System.Text.Json;
using System.Text;
using System.Text.Json;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_CartService.MessageQueue.Messages;
using LayeredArchitecture_Task2_Catalog_Service.MessageQueue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LayeredArchitecture_Task1_CartService.MessageQueue.Implementation;

public class ProductUpdatedConsumerService : BackgroundService
{
    private readonly ICartService _cartService;
    private readonly RabbitMQOptions _options;
    private readonly ILogger<ProductUpdatedConsumerService> _logger;

    private IConnection? _connection;
    private IChannel? _channel;

    public ProductUpdatedConsumerService(
        ICartService cartService,
        IOptions<RabbitMQOptions> options,
        ILogger<ProductUpdatedConsumerService> logger)
    {
        _cartService = cartService;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = _options.ProductUpdated;

        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(settings.Exchange, ExchangeType.Direct, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(settings.Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(settings.Queue, settings.Exchange, settings.RoutingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                var productUpdated = JsonSerializer.Deserialize<ProductUpdatedMessage>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (productUpdated is null)
                {
                    _logger.LogWarning("Received null or invalid product updated message.");
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
                    return;
                }

                _logger.LogInformation("Updating cart items for product {ProductId} with new name '{Name}' and price {Price}.",
                    productUpdated.Id, productUpdated.Name, productUpdated.Price);

                await _cartService.UpdateItemsByProductIdAsync(
                    productUpdated.Id,
                    productUpdated.Name,
                    productUpdated.Price,
                    productUpdated.ImageUrl,
                    productUpdated.ImageAltText);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing product updated message: {Message}", body);
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(settings.Queue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
            await _channel.CloseAsync();
        if (_connection is not null)
            await _connection.CloseAsync();

        await base.StopAsync(cancellationToken);
    }
}
