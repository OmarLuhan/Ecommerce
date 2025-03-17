using System;
using System.Collections.Generic;

namespace Ecommerce.api.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public decimal Price { get; set; }

    public decimal OfferPrice { get; set; }

    public int Stock { get; set; }

    public string? Image { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual Category? Category { get; set; }
}
