using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Bill : BaseEntity
{
    public int BillId { get; set; }

    public int? PaymentId { get; set; }

    public int? ServiceOrderId { get; set; }

    public int? OrderId { get; set; }

    public double? Price { get; set; }

    public string? Description { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ServiceOrder? ServiceOrder { get; set; }
}
