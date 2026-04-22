using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LayeredArchitecture_Task1_Cart_Service.Tests;

public class RemoveFromCartTests
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
    public async Task RemoveFromCart_ReturnsOkResult()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        _cartServiceMock
            .Setup(s => s.RemoveFromCartAsync(cartId, itemId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RemoveFromCart(cartId, itemId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task RemoveFromCart_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        _cartServiceMock
            .Setup(s => s.RemoveFromCartAsync(cartId, itemId))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.RemoveFromCart(cartId, itemId);

        // Assert
        _cartServiceMock.Verify(s => s.RemoveFromCartAsync(cartId, itemId), Times.Once);
    }

    [Test]
    public void RemoveFromCart_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        _cartServiceMock
            .Setup(s => s.RemoveFromCartAsync(cartId, itemId))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () =>
            await _controller.RemoveFromCart(cartId, itemId));
    }
}