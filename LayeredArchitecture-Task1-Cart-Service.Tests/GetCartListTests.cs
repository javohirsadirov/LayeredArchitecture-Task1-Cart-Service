using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Controllers;
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
    public async Task GetCartList_ReturnsOkResult_WithCartItems()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var expectedItems = new List<CartDto>
        {
            new CartDto
            {
                Id = cartId,
                CartItems =
                [
                    new ItemDto { Id = Guid.NewGuid(), Name = "Item1", Quantity = 1, Price = 9.99m }
                ]
            }
        };
        _cartServiceMock
            .Setup(s => s.GetCartListAsync(cartId))
            .ReturnsAsync(expectedItems);

        // Act
        var result = await _controller.GetCartList(cartId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expectedItems));
    }

    [Test]
    public async Task GetCartList_ReturnsOkResult_WithEmptyList_WhenCartIsEmpty()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        _cartServiceMock
            .Setup(s => s.GetCartListAsync(cartId))
            .ReturnsAsync(new List<CartDto>());

        // Act
        var result = await _controller.GetCartList(cartId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.InstanceOf<List<CartDto>>());
    }

    [Test]
    public async Task GetCartList_CallsServiceWithCorrectCartId()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        _cartServiceMock
            .Setup(s => s.GetCartListAsync(cartId))
            .ReturnsAsync(new List<CartDto>());

        // Act
        await _controller.GetCartList(cartId);

        // Assert
        _cartServiceMock.Verify(s => s.GetCartListAsync(cartId), Times.Once);
    }
}