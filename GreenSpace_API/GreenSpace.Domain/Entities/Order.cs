using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public double? TotalAmount { get; set; }

    public int? PaymentId { get; set; }

    public int? Status { get; set; }

    public int? Ship { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Payment? Payment { get; set; }

    public virtual User? User { get; set; }
}
