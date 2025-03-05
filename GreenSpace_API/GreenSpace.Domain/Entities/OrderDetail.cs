using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Material Product { get; set; } = null!;
}
