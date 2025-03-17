using System;
using System.Collections.Generic;

namespace Ecommerce.api.Model;

public partial class Sale
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Total { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual User User { get; set; } = null!;
}
