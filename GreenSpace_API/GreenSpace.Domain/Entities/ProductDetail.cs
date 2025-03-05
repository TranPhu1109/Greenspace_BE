using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class ProductDetail
{
    public int ProductId { get; set; }

    public int DesignIdeaId { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public virtual DesignIdea DesignIdea { get; set; } = null!;

    public virtual Material Product { get; set; } = null!;
}
