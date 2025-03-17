using System;
using System.Collections.Generic;

namespace Ecommerce.api.Model;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
