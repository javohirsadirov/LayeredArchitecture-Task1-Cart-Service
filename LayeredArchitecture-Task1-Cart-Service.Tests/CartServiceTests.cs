using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Exceptions;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Implementation;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;
using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Repository.Models;
using Moq;

namespace LayeredArchitecture_Task1_Cart_Service.Tests;

public class CartServiceTests
{
    private Mock<ICartRepository> _cartRepositoryMock;
    private ICartService _cartService;

    [SetUp]
    public void Setup()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _cartService = new CartService(_cartRepositoryMock.Object);
    }

    [Test]
    public async Task AddItemAsync_ThrowsValidationException_WhenPriceIsNegative()
    {
        var item = new ItemDto { Id = 1, Name = "Item", Quantity = 1, Price = -5m };

        Assert.ThrowsAsync<ValidationException>(async () =>
            await _cartService.AddItemAsync("cart-1", item));
    }

    [Test]
    public async Task AddItemAsync_ThrowsValidationException_WhenQuantityIsZero()
    {
        var item = new ItemDto { Id = 1, Name = "Item", Quantity = 0, Price = 10m };

        Assert.ThrowsAsync<ValidationException>(async () =>
            await _cartService.AddItemAsync("cart-1", item));
    }

    [Test]
    public async Task AddItemAsync_ThrowsValidationException_WhenNameIsEmpty()
    {
        var item = new ItemDto { Id = 1, Name = "  ", Quantity = 1, Price = 10m };

        Assert.ThrowsAsync<ValidationException>(async () =>
            await _cartService.AddItemAsync("cart-1", item));
    }

    [Test]
    public async Task AddItemAsync_CallsRepository_WhenValid()
    {
        var item = new ItemDto { Id = 1, Name = "Valid Item", Quantity = 2, Price = 10m };
        _cartRepositoryMock
            .Setup(r => r.AddItemAsync("cart-1", It.IsAny<Item>()))
            .Returns(Task.CompletedTask);

        await _cartService.AddItemAsync("cart-1", item);

        _cartRepositoryMock.Verify(r => r.AddItemAsync("cart-1", It.Is<Item>(i =>
            i.Id == 1 && i.Name == "Valid Item" && i.Quantity == 2 && i.Price == 10m)), Times.Once);
    }

    [Test]
    public async Task UpdateItemsByProductIdAsync_CallsRepository()
    {
        _cartRepositoryMock
            .Setup(r => r.UpdateItemsByProductIdAsync(1, "Updated", 20m, "http://img.png", "alt"))
            .Returns(Task.CompletedTask);

        await _cartService.UpdateItemsByProductIdAsync(1, "Updated", 20m, "http://img.png", "alt");

        _cartRepositoryMock.Verify(r => r.UpdateItemsByProductIdAsync(1, "Updated", 20m, "http://img.png", "alt"), Times.Once);
    }

    [Test]
    public async Task GetCartAsync_ReturnsNull_WhenCartDoesNotExist()
    {
        _cartRepositoryMock
            .Setup(r => r.GetCartAsync("nonexistent"))
            .ReturnsAsync((Cart?)null);

        var result = await _cartService.GetCartAsync("nonexistent");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCartAsync_ReturnsCartDto_WhenCartExists()
    {
        var cart = new Cart
        {
            Key = "cart-1",
            Items = [new Item { Id = 1, CartKey = "cart-1", Name = "Item1", Quantity = 1, Price = 5m }]
        };
        _cartRepositoryMock
            .Setup(r => r.GetCartAsync("cart-1"))
            .ReturnsAsync(cart);

        var result = await _cartService.GetCartAsync("cart-1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Key, Is.EqualTo("cart-1"));
        Assert.That(result.Items, Has.Count.EqualTo(1));
        Assert.That(result.Items[0].Name, Is.EqualTo("Item1"));
    }

    [Test]
    public async Task RemoveItemAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        _cartRepositoryMock
            .Setup(r => r.RemoveItemAsync("cart-1", 1))
            .ReturnsAsync(true);

        var result = await _cartService.RemoveItemAsync("cart-1", 1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task RemoveItemAsync_ReturnsFalse_WhenRepositoryReturnsFalse()
    {
        _cartRepositoryMock
            .Setup(r => r.RemoveItemAsync("cart-1", 999))
            .ReturnsAsync(false);

        var result = await _cartService.RemoveItemAsync("cart-1", 999);

        Assert.That(result, Is.False);
    }
}
