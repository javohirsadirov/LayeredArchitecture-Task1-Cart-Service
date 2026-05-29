namespace LayeredArchitecture_Task1_CartService.MessageQueue.Messages;

public class ProductUpdatedMessage
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
}
