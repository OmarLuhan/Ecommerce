using System;
using System.Collections.Generic;

namespace Ecommerce.api.Model;

public partial class SaleDetail
{
    public int Id { get; set; }

    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
