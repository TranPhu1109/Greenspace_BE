namespace GreenSpace.Domain.Entities;

public class ServiceOrderDetail : BaseEntity
{
    public Guid ServiceOrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public double TotalPrice { get; set; }

    public virtual Material Product { get; set; } = null!;

    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
}
