using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class ServiceOrderDetail
{
    public int ServiceOrderId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public double? TotalPrice { get; set; }

    public virtual Material Product { get; set; } = null!;

    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
}
