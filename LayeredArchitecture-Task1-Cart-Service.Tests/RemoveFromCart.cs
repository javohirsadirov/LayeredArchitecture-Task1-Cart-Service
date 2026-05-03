using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Controllers.V1;
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
    public async Task DeleteItem_ReturnsOk_WhenItemExists()
    {
        // Arrange
        var key = "cart-1";
        var itemId = 1;
        _cartServiceMock
            .Setup(s => s.RemoveItemAsync(key, itemId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteItem(key, itemId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task DeleteItem_ReturnsOk_WhenItemDoesNotExist()
    {
        // Arrange
        var key = "cart-1";
        var itemId = 999;
        _cartServiceMock
            .Setup(s => s.RemoveItemAsync(key, itemId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteItem(key, itemId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task DeleteItem_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var key = "cart-1";
        var itemId = 1;
        _cartServiceMock
            .Setup(s => s.RemoveItemAsync(key, itemId))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.DeleteItem(key, itemId);

        // Assert
        _cartServiceMock.Verify(s => s.RemoveItemAsync(key, itemId), Times.Once);
    }
}