using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Controllers.V1;
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
    public async Task AddItem_ReturnsOkResult()
    {
        // Arrange
        var key = "cart-1";
        var item = new ItemDto { Id = 1, Name = "Test Item", Quantity = 1, Price = 9.99m };
        _cartServiceMock
            .Setup(s => s.AddItemAsync(key, item))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddItem(key, item);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task AddItem_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var key = "cart-1";
        var item = new ItemDto { Id = 2, Name = "Test Item", Quantity = 2, Price = 19.99m };
        _cartServiceMock
            .Setup(s => s.AddItemAsync(key, item))
            .Returns(Task.CompletedTask);

        // Act
        await _controller.AddItem(key, item);

        // Assert
        _cartServiceMock.Verify(s => s.AddItemAsync(key, item), Times.Once);
    }

    [Test]
    public void AddItem_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var key = "cart-1";
        var item = new ItemDto { Id = 1, Name = "Test Item", Quantity = 1, Price = 5.00m };
        _cartServiceMock
            .Setup(s => s.AddItemAsync(key, item))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () =>
            await _controller.AddItem(key, item));
    }
}