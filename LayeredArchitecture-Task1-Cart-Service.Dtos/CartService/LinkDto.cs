// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.Dtos.CartService;

/// <summary>
/// Represents a HATEOAS link.
/// </summary>
public class LinkDto
{
    /// <summary>Gets or sets the URL of the link.</summary>
    public required string Href { get; set; }

    /// <summary>Gets or sets the relation type of the link.</summary>
    public required string Rel { get; set; }

    /// <summary>Gets or sets the HTTP method for the link.</summary>
    public required string Method { get; set; }
}
