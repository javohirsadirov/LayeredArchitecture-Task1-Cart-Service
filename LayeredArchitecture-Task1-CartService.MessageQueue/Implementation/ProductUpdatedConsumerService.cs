// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using System.Text;
using System.Text.Json;
using LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;
using LayeredArchitectureTask1CartService.MessageQueue.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LayeredArchitectureTask1CartService.MessageQueue.Implementation;

/// <summary>
/// Background service that listens for product update messages from RabbitMQ
/// and updates the cart items accordingly.
/// </summary>
public partial class ProductUpdatedConsumerService : BackgroundService
{
    private readonly ICartService cartService;
    private readonly RabbitMQOptions rabbitMQOptions;
    private readonly ILogger<ProductUpdatedConsumerService> logger;

    private IConnection? connection;
    private IChannel? channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductUpdatedConsumerService"/> class.
    /// </summary>
    /// <param name="cartService">The cart service.</param>
    /// <param name="rabbitMQOptions">The RabbitMQ options.</param>
    /// <param name="logger">The logger.</param>
    public ProductUpdatedConsumerService(
        ICartService cartService,
        IOptions<RabbitMQOptions> rabbitMQOptions,
        ILogger<ProductUpdatedConsumerService> logger)
    {
        this.cartService = cartService;
        this.rabbitMQOptions = rabbitMQOptions.Value;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (this.channel is not null)
        {
            await this.channel.CloseAsync(cancellationToken);
        }

        if (this.connection is not null)
        {
            await this.connection.CloseAsync(cancellationToken);
        }

        await base.StopAsync(cancellationToken);
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = this.rabbitMQOptions.ProductUpdated;

        var factory = new ConnectionFactory
        {
            HostName = this.rabbitMQOptions.HostName,
            Port = this.rabbitMQOptions.Port,
        };

        this.connection = await factory.CreateConnectionAsync(stoppingToken);
        this.channel = await this.connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await this.channel.ExchangeDeclareAsync(settings.Exchange, ExchangeType.Direct, durable: true, cancellationToken: stoppingToken);
        await this.channel.QueueDeclareAsync(settings.Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await this.channel.QueueBindAsync(settings.Queue, settings.Exchange, settings.RoutingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(this.channel);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                var productUpdated = JsonSerializer.Deserialize<ProductUpdatedMessage>(body, jsonSerializerOptions);

                if (productUpdated is null)
                {
                    LogReceivedNullMessage(this.logger);
                    await this.channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
                    return;
                }

                LogUpdatingCartItems(this.logger, productUpdated.Id, productUpdated.Name, productUpdated.Price);

                await this.cartService.UpdateItemsByProductIdAsync(
                    productUpdated.Id,
                    productUpdated.Name,
                    productUpdated.Price,
                    productUpdated.ImageUrl,
                    productUpdated.ImageAltText);

                await this.channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
            }
            catch (Exception ex)
            {
                LogProcessingError(this.logger, ex, body);
                await this.channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await this.channel.BasicConsumeAsync(settings.Queue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Received null or invalid product updated message.")]
    private static partial void LogReceivedNullMessage(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Updating cart items for product {ProductId} with new name '{Name}' and price {Price}.")]
    private static partial void LogUpdatingCartItems(ILogger logger, int productId, string name, decimal price);

    [LoggerMessage(Level = LogLevel.Error, Message = "Error processing product updated message: {Body}")]
    private static partial void LogProcessingError(ILogger logger, Exception ex, string body);
}
