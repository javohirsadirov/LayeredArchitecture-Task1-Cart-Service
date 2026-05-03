using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Controllers.V1;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LayeredArchitecture_Task1_Cart_Service.Tests;

public class GetCartListTests
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
    public async Task GetCart_ReturnsOkResult_WithCart()
    {
        // Arrange
        var key = "cart-1";
        var expectedCart = new CartDto
        {
            Key = key,
            Items = [new ItemDto { Id = 1, Name = "Item1", Quantity = 1, Price = 9.99m }]
        };
        _cartServiceMock
            .Setup(s => s.GetCartAsync(key))
            .ReturnsAsync(expectedCart);

        // Act
        var result = await _controller.GetCart(key);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expectedCart));
    }

    [Test]
    public async Task GetCart_ReturnsNotFound_WhenCartDoesNotExist()
    {
        // Arrange
        var key = "nonexistent";
        _cartServiceMock
            .Setup(s => s.GetCartAsync(key))
            .ReturnsAsync((CartDto?)null);

        // Act
        var result = await _controller.GetCart(key);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task GetCart_CallsServiceWithCorrectKey()
    {
        // Arrange
        var key = "cart-1";
        _cartServiceMock
            .Setup(s => s.GetCartAsync(key))
            .ReturnsAsync(new CartDto { Key = key });

        // Act
        await _controller.GetCart(key);

        // Assert
        _cartServiceMock.Verify(s => s.GetCartAsync(key), Times.Once);
    }
}