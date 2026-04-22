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
        var expectedCart = new CartDto
        {
            Id = cartId,
            CartItems =
            [
                new ItemDto { Id = Guid.NewGuid(), Name = "Item1", Quantity = 1, Price = 9.99m }
            ]
        };
        _cartServiceMock
            .Setup(s => s.GetCartListAsync(cartId))
            .ReturnsAsync(expectedCart);

        // Act
        var result = await _controller.GetCartList(cartId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expectedCart));
    }

    [Test]
    public async Task GetCartList_ReturnsOkResult_WithEmptyCartItems_WhenCartIsEmpty()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var expectedCart = new CartDto { Id = cartId };
        _cartServiceMock
            .Setup(s => s.GetCartListAsync(cartId))
            .ReturnsAsync(expectedCart);

        // Act
        var result = await _controller.GetCartList(cartId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        var cart = okResult.Value as CartDto;
        Assert.That(cart, Is.Not.Null);
        Assert.That(cart!.CartItems, Is.Empty);
    }

    [Test]
    public async Task GetCartList_CallsServiceWithCorrectCartId()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        _cartServiceMock
            .Setup(s => s.GetCartListAsync(cartId))
            .ReturnsAsync(new CartDto { Id = cartId });

        // Act
        await _controller.GetCartList(cartId);

        // Assert
        _cartServiceMock.Verify(s => s.GetCartListAsync(cartId), Times.Once);
    }
}