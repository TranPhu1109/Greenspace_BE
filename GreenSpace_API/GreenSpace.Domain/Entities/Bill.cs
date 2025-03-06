namespace GreenSpace.Domain.Entities;

public class Bill : BaseEntity
{

    public Guid PaymentId { get; set; }

    public Guid ServiceOrderId { get; set; }

    public Guid OrderId { get; set; }

    public double Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public virtual Order? Order { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ServiceOrder? ServiceOrder { get; set; }
}
