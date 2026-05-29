using System.Text.Json;
using LayeredArchitecture_Task1_CartService.MessageQueue.Interfaces;
using LayeredArchitecture_Task1_CartService.MessageQueue.Messages;
using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.MessageQueue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LayeredArchitecture_Task1_CartService.MessageQueue.Implementation;

public class ProductUpdatedConsumerService : BackgroundService
{
    private readonly IMessageConsumer _consumer;
    private readonly ICartRepository _cartRepository;
    private readonly RabbitMQOptions _options;
    private readonly ILogger<ProductUpdatedConsumerService> _logger;

    public ProductUpdatedConsumerService(
        IMessageConsumer consumer,
        ICartRepository cartRepository,
        IOptions<RabbitMQOptions> options,
        ILogger<ProductUpdatedConsumerService> logger)
    {
        _consumer = consumer;
        _cartRepository = cartRepository;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = _options.ProductUpdated;

        await _consumer.StartAsync(
            settings.Exchange,
            settings.Queue,
            settings.RoutingKey,
            async message =>
            {
                try
                {
                    var productUpdated = JsonSerializer.Deserialize<ProductUpdatedMessage>(message, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (productUpdated is null)
                    {
                        _logger.LogWarning("Received null or invalid product updated message.");
                        return;
                    }

                    _logger.LogInformation("Updating cart items for product {ProductId} with new name '{Name}' and price {Price}.",
                        productUpdated.Id, productUpdated.Name, productUpdated.Price);

                    await _cartRepository.UpdateItemsByProductIdAsync(
                        productUpdated.Id,
                        productUpdated.Name,
                        productUpdated.Price,
                        productUpdated.ImageUrl,
                        productUpdated.ImageAltText);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing product updated message: {Message}", message);
                }
            },
            stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _consumer.StopAsync();
        await base.StopAsync(cancellationToken);
    }
}
