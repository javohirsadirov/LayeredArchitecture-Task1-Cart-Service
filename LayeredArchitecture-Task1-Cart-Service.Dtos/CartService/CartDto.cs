namespace LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

public class CartDto
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public List<ItemDto> CartItems { get; set; } = [];
}