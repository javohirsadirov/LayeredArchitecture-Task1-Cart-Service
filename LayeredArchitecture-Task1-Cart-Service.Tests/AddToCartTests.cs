using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Controllers;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LayeredArchitecture_Task1_Cart_Service.Tests;

public class AddToCartTests
{
    private Mock<ICartService> _cartServiceMock;
    private CartController _controller;

    [SetUp]
    public void Setup()
    {
        _cartServiceMock = new Mock<ICartService>();
        _controller = new CartController(_cartServiceMock.Object);
    }

    [Test]
    public async Task AddToCart_ReturnsOkResult()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var item = new ItemDto { Id = Guid.NewGuid(), Name = "Test Item", Quantity = 1, Price = 9.99m };
        _cartServiceMock
            .Setup(s => s.AddToCartAsync(cartId, item))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddToCart(cartId, item);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task AddToCart_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var item = new ItemDto { Id = Guid.NewGuid(), Name = "Test Item", Quantity = 2, Price = 19.99m };
        _cartServiceMock
            .Setup(s => s.AddToCartAsync(cartId, item))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.AddToCart(cartId, item);

        // Assert
        _cartServiceMock.Verify(s => s.AddToCartAsync(cartId, item), Times.Once);
    }

    [Test]
    public void AddToCart_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var item = new ItemDto { Id = Guid.NewGuid(), Name = "Test Item", Quantity = 1, Price = 5.00m };
        _cartServiceMock
            .Setup(s => s.AddToCartAsync(cartId, item))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () =>
            await _controller.AddToCart(cartId, item));
    }
}