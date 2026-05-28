namespace LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

/// <summary>
/// Represents a HATEOAS link.
/// </summary>
public class LinkDto
{
    /// <summary>The URL of the link.</summary>
    public required string Href { get; set; }

    /// <summary>The relation type of the link.</summary>
    public required string Rel { get; set; }

    /// <summary>The HTTP method for the link.</summary>
    public required string Method { get; set; }
}
