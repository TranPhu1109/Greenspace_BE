using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class ServiceOrder
{
    public int ServiceOrderId { get; set; }

    public Guid? UserId { get; set; }

    public int? DesignIdeaId { get; set; }

    public string? Address { get; set; }

    public string? CusPhone { get; set; }

    public double? EreaSize { get; set; }

    public double? TotalCost { get; set; }

    public int? PaymentId { get; set; }

    public int? ServiceTypeId { get; set; }

    public bool? IsCustom { get; set; }

    public double? DesignPrice { get; set; }

    public double? MasterPrice { get; set; }

    public string? Description { get; set; }

    public int? ImageId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();

    public virtual ServiceType? ServiceType { get; set; }

    public virtual ICollection<WorkTask> WorkTask { get; set; } = new List<WorkTask>();
}
