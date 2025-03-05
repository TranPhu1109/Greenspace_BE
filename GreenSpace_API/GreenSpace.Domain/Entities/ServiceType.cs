using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class ServiceType
{
    public int ServiceTypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
}
