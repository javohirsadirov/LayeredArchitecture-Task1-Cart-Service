namespace LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

public class ItemDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }
}