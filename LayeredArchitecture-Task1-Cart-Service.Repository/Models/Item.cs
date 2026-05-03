namespace LayeredArchitecture_Task1_Cart_Service.Repository.Models;

public class Item
{
    public required int Id { get; set; }
    public required string CartKey { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }
}