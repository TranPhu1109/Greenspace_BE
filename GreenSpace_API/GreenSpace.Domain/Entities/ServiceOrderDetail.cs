namespace GreenSpace.Domain.Entities;

public class ServiceOrderDetail : BaseEntity
{
    public Guid ServiceOrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
}
